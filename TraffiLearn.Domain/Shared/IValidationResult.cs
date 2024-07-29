namespace TraffiLearn.Domain.Shared
{
    public interface IValidationResult
    {
        public static readonly Error ValidationError =
            Error.Validation(
                code: "ValidationError",
                description: "A validation problem occurred.");

        Error[] Errors { get; }
    }
}
