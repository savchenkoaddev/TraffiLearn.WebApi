using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Common.Price
{
    public static class PriceErrors
    {
        public static readonly Error NegativeAmount =
            Error.Validation(
                code: "Price.NegativeAmount",
                description: "Amount cannot be less than zero.");
    }
}
