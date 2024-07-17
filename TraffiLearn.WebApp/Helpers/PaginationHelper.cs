namespace TraffiLearn.WebApp.Helpers
{
    public sealed class PaginationHelper<T>
    {
        public int TotalItems { get; }
        public int PageSize { get; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        public PaginationHelper(int totalItems, int pageSize)
        {
            TotalItems = totalItems;
            PageSize = pageSize;
        }

        public IEnumerable<T> GetPaginatedData(IEnumerable<T> data, int page)
        {
            return data
                .Skip((page - 1) * PageSize)
                .Take(PageSize);
        }
    }
}
