namespace TraffiLearn.Application.Abstractions.Payments
{
    public sealed record CreateCheckoutSessionRequest(
        string ProductName,
        decimal Amount,
        string Currency,
        int Quantity,
        string PaymentMode);
}
