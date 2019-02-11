using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using NodaTime;
using NodaTime.Serialization.JsonNet;

namespace Roger.Json
{
    public static class SerializerSettings
    {
        public static JsonSerializerSettings Default
        {
            get
            {
                DefaultContractResolver contractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new CamelCaseNamingStrategy()
                };
                var settings = new JsonSerializerSettings()
                {
                    MissingMemberHandling = MissingMemberHandling.Ignore,
                    NullValueHandling = NullValueHandling.Ignore,
                    DefaultValueHandling = DefaultValueHandling.Include,
                    MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                    DateParseHandling = DateParseHandling.None,
                    ContractResolver = contractResolver
                };
                settings.Converters.Add(new StringEnumConverter());
                settings.ConfigureForNodaTime(DateTimeZoneProviders.Tzdb);
                return settings;
            }
        }
    }
}
