namespace TraffiLearn.SharedKernel.Shared
{
    public sealed class ValidationResult : Result, IValidationResult
    {
        private ValidationResult(Error[] errors) : base(
            isSuccess: false,
            error: IValidationResult.ValidationError)
        {
            Errors = errors;
        }

        public Error[] Errors { get; }

        public static ValidationResult WithErrors(Error[] errors) => new(errors);
    }
}
