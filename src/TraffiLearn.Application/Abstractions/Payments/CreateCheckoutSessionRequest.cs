namespace TraffiLearn.Application.Abstractions.Payments
{
    public sealed record CreateCheckoutSessionRequest(
        string ProductName,
        long Amount,
        string Currency);
}
