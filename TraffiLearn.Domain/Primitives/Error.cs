namespace TraffiLearn.Domain.Primitives
{
    public record Error
    {
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

        public static readonly Error NullValue = new("Error.NullValue", "Null value was provided", ErrorType.Failure);

        private Error(
            string code,
            string description,
            ErrorType errorType)
        {
            Code = code;
            Description = description;
            ErrorType = errorType;
        }

        public string Code { get; init; }

        public string Description { get; init; }

        public ErrorType ErrorType { get; init; }

        public static Error Failure(string code, string description) =>
            new Error(code, description, ErrorType.Failure);

        public static Error NotFound(string code, string description) =>
            new Error(code, description, ErrorType.NotFound);

        public static Error Validation(string code, string description) =>
            new Error(code, description, ErrorType.Validation);
    }
}
