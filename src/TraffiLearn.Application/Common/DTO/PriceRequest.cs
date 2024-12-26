using TraffiLearn.SharedKernel.ValueObjects.Prices;

namespace TraffiLearn.Application.Common.DTO
{
    public sealed record PriceRequest(
        decimal Amount,
        Currency Currency);
}
