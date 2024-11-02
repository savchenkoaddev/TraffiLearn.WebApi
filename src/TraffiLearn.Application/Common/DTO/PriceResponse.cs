namespace TraffiLearn.Application.Common.DTO
{
    public sealed record PriceResponse(
        decimal Amount,
        string Currency);
}
