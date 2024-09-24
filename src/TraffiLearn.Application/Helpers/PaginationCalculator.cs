namespace TraffiLearn.Application.Helpers
{
    internal static class PaginationCalculator
    {
        public static int CalculateTotalPages(
            int pageSize,
            int itemsCount)
        {
            return (int)Math.Ceiling((double)itemsCount / pageSize);
        }
    }
}
