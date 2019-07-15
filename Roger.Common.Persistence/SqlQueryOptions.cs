namespace Roger.Common.Persistence
{
    public class SqlQueryOptions
    {
        private int _pageSize;
        private int _pageNumber;
        public string PartitionKey { get; set; }

        public int PageNumber
        {
            get => _pageNumber;
            set => _pageNumber = value < 1 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value < 0 ? 10 : value;
        }

        public bool RequiresTotalCount => PageNumber == 1;

        public string ContinuationToken { get; set; }

        public int PageIndex => PageNumber - 1;
    }
}