namespace TraffiLearn.Application.UseCases.SubscriptionPlans.DTO
{
    public sealed record RenewalPeriodResponse(
        int Interval,
        string Type);
}
