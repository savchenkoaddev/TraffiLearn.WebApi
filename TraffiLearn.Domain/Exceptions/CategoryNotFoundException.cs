namespace TraffiLearn.Domain.Exceptions
{
    public sealed class CategoryNotFoundException : Exception
    {
        public CategoryNotFoundException() : base("Category with the provided id has not been found.")
        { }

        public CategoryNotFoundException(Guid categoryId) : base($"Category with the '{categoryId}' id has not been found.")
        { }
    }
}
