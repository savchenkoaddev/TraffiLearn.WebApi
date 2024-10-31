using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.SharedKernel.ValueObjects.Prices
{
    public static class PriceErrors
    {
        public static readonly Error NegativeAmount =
            Error.Validation(
                code: "Price.NegativeAmount",
                description: "Amount cannot be less than zero.");
    }
}
