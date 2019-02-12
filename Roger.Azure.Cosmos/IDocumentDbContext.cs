using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Roger.Azure.Cosmos.Attributes;
using Roger.Azure.Cosmos.Configuration;
using Roger.Json;

namespace Roger.Azure.Cosmos
{
    public interface IDocumentDbContext
    {
        DocumentClient Client { get; }
        string DatabaseName { get; }
        Uri DatabaseUri { get; }
        Database Db { get; }
    }

    public class DocumentDbContext : IDocumentDbContext
    {
        private readonly ILogger _logger;
        private readonly DocumentDbConfiguration _configuration;
        public DocumentClient Client { get; }
        public Database Db { get; }
        public string DatabaseName { get; }
        public Uri DatabaseUri { get; }
        public DocumentDbContext(IOptions<DocumentDbConfiguration> options, ILogger<DocumentDbContext> logger)
        {
            _logger = logger;
            _configuration = options.Value;
            _logger.LogInformation($"Creating database {_configuration.DatabaseName} if not exists in {_configuration.EndpointUrl}");
            Client = new DocumentClient(_configuration.EndpointUri, _configuration.PrimaryKey, SerializerSettings.Default);
            var resDb = Client.CreateDatabaseIfNotExistsAsync(new Database() { Id = _configuration.DatabaseName }).Result;
            Db = resDb.Resource;
            DatabaseName = _configuration.DatabaseName;
            DatabaseUri = UriFactory.CreateDatabaseUri(_configuration.DatabaseName);
        }
    }

    public interface IDocumentDbRepository<T>
        where T : new()
    {
        Task<T> CreateAsync(T doc);
        Task<T> CreateOrUpdateAsync(T doc);
        Task<T> UpdateAsync(string id, T doc);
        Task DeleteAsync(string id);
        Task<T> GetByIdAsync(string id, bool throwException = true);
        List<T> Get(int maxItemCount = 10, string continuationToken = null);
        List<T> Get(Expression<Func<T, bool>> predicate, int maxItemCount = 10, string continuationToken = null);
    }

    public abstract class DocumentDbRepository<T, U> : IDocumentDbRepository<T>
        where T : new()
    {
        private readonly string _collectionName;
        private readonly Uri CollectionUri;

        private readonly IDocumentDbContext _context;
        protected readonly ILogger Logger;
        protected readonly DocumentCollection DocumentCollection;

        public DocumentDbRepository(IDocumentDbContext context, ILogger logger)
        {
            _context = context;
            Logger = logger;
            var attr = GetAttribute();
            _collectionName = attr?.Name;
            Logger.LogInformation($"Creating collection {_collectionName} if not exists under database {context.DatabaseUri}");
            var resDc = _context.Client.CreateDocumentCollectionIfNotExistsAsync(context.DatabaseUri,
                new DocumentCollection() { Id = _collectionName, DefaultTimeToLive = attr?.DefaultTimeToLive }).Result;
            CollectionUri = UriFactory.CreateDocumentCollectionUri(_context.DatabaseName, _collectionName);
            DocumentCollection = resDc.Resource;
        }

        public async Task<T> CreateAsync(U model)
        {
            var response = await _context.Client.CreateDocumentAsync(CollectionUri, model);
            return response.Resource.ToString().Deserialize<T>();
        }

        public async Task<T> CreateOrUpdateAsync(U model)
        {
            var response = await _context.Client.UpsertDocumentAsync(CollectionUri, model);
            return ToModel(response.Resource.ToString().Deserialize<T>());
        }


        public async Task<T> UpdateAsync(string id, U model)
        {
            var response = await _context.Client.ReplaceDocumentAsync(GetDocumentUri(id), model);
            return ToModel(response.Resource.ToString().Deserialize<T>());
        }

        public async Task<T> GetByIdAsync(string id, bool throwException = true)
        {
            try
            {
                var response = await _context.Client.ReadDocumentAsync(GetDocumentUri(id));
                return ToModel(response.Resource.ToString().Deserialize<T>());
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == HttpStatusCode.NotFound && !throwException)
                {
                    return default(T);
                }

                throw;
            }
        }

        protected List<T> GetDocuments(int maxItemCount = 10, string continuationToken = null)
        {
            return GetQueryable(new FeedOptions()
            {
                MaxItemCount = maxItemCount,
                RequestContinuation = continuationToken
            })
                .ToList();
        }

        public List<T> Get(int maxItemCount = 10, string continuationToken = null)
        {
            return GetDocuments(maxItemCount, continuationToken)
                .ToList();
        }

        protected List<T> GetDocuments(Expression<Func<T, bool>> predicate, int maxItemCount = 10, string continuationToken = null)
        {
            return GetQueryable(new FeedOptions()
            {
                MaxItemCount = maxItemCount,
                RequestContinuation = continuationToken
            })
                .Where(predicate)
                .ToList();
        }

        public List<T> Get(Expression<Func<T, bool>> predicate, int maxItemCount = 10, string continuationToken = null)
        {
            return GetDocuments(predicate, maxItemCount, continuationToken)
                .ToList();
        }

        protected List<T> GetDocuments(string sqlQuery, int maxItemCount = 10, string continuationToken = null)
        {
            return _context.Client.CreateDocumentQuery<T>(DocumentCollection.SelfLink, sqlQuery, new FeedOptions()
            {
                MaxItemCount = maxItemCount,
                RequestContinuation = continuationToken
            })
                .ToList();
        }

        public List<T> Get(string sqlQuery, int maxItemCount = 10, string continuationToken = null)
        {
            return GetDocuments(sqlQuery, maxItemCount, continuationToken)
                .ToList();
        }

        public Task DeleteAsync(string id)
        {
            return _context.Client.DeleteDocumentAsync(GetDocumentUri(id));
        }

        private Uri GetDocumentUri(string id)
        {
            return UriFactory.CreateDocumentUri(_context.DatabaseName, _collectionName, id);
        }

        public IQueryable<T> GetQueryable(FeedOptions options = null)
        {
            return _context.Client.CreateDocumentQuery<T>(CollectionUri, options);
        }

        private CollectionNameAttribute GetAttribute()
        {
            return (CollectionNameAttribute)Attribute.GetCustomAttribute(typeof(T), typeof(CollectionNameAttribute));
        }
    }
}
