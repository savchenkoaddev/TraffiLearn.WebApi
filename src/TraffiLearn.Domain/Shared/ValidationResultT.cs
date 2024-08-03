namespace TraffiLearn.Domain.Shared
{
    public sealed class ValidationResult<TValue> : Result<TValue>, IValidationResult
    {
        private ValidationResult(Error[] errors): base(
            value: default, 
            isSuccess: false, 
            error: IValidationResult.ValidationError)
        {
            Errors = errors;
        }

        public Error[] Errors { get; }

        public static ValidationResult<TValue> WithErrors(Error[] errors) => new(errors);
    }
}
