namespace EShop.Shared.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Data { get; set; } = new List<T>();
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }

        // Calculated automatically from TotalCount and PageSize
        public int TotalPages => (int)Math.Ceiling(
            (double)TotalCount / PageSize);

        public bool HasNextPage => Page < TotalPages;
        public bool HasPreviousPage => Page > 1;

        // Static factory method - easy to create
        public static PagedResult<T> Create(
            IEnumerable<T> data,
            int totalCount,
            int page,
            int pageSize)
        {
            return new PagedResult<T>
            {
                Data = data,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }
    }
}