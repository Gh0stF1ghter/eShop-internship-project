namespace Catalogs.Domain.RequestFeatures
{
    public abstract class RequestParameters
    {
        const int maxPageSize = 50;

        private int _pageSize = 10;
        public int PageSize
        {
            get =>
                _pageSize;
            set =>
                _pageSize = value < maxPageSize ? value : maxPageSize;
        }

        public int PageNumber { get; set; } = 1;

        public string? Fields { get; set; }
    }
}
