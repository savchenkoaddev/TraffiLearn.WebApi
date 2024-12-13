namespace TraffiLearn.Application.Abstractions.Payments
{
    public interface IPaymentService
    {
        Task<Uri> CreateCheckoutSessionAsync(
            CreateCheckoutSessionRequest request);
    }
}
