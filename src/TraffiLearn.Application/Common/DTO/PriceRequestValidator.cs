using FluentValidation;

namespace TraffiLearn.Application.Common.DTO
{
    internal sealed class PriceRequestValidator : AbstractValidator<PriceRequest>
    {
        public PriceRequestValidator()
        {
            RuleFor(x => x.Amount)
                .NotNull()
                .GreaterThan(0);

            RuleFor(x => x.Currency)
                .NotEmpty()
                .IsInEnum().WithMessage("Currency must be one of the supported currencies.");
        }
    }
}
