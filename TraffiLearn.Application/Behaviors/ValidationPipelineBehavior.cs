using FluentValidation;
using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Behaviors
{
    public sealed class ValidationPipelineBehavior<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest
        where TResponse: Result
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationPipelineBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request, 
            RequestHandlerDelegate<TResponse> next, 
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
            {
                return await next();
            }   

            Error[] errors = GetErrors(request);

            if (errors.Any())
            {
                return CreateValidationResult<TResponse>(errors);
            }

            return await next();
        }

        private Error[] GetErrors(TRequest request)
        {
            return _validators
                .Select(validator => validator.Validate(request))
                .SelectMany(validationResult => validationResult.Errors)
                .Where(validationFailure => validationFailure is not null)
                .Select(failure => Error.Validation(
                    code: failure.PropertyName,
                    description: failure.ErrorMessage))
                .Distinct()
                .ToArray();
        }

        private static TResult CreateValidationResult<TResult>(Error[] errors)
            where TResult : Result
        {
            if (typeof(TResult) == typeof(Result))
            {
                return (ValidationResult
                    .WithErrors(errors) as TResult)!;
            }

            object validationResult = typeof(ValidationResult<>)
                .GetGenericTypeDefinition()
                .MakeGenericType(typeof(TResult).GenericTypeArguments.First())
                .GetMethod(nameof(ValidationResult.WithErrors))!
                .Invoke(null, new object?[] { errors })!;

            return (TResult) validationResult;
        }
    }
}
