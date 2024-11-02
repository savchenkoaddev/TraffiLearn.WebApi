namespace TraffiLearn.Application.SubscriptionPlans.DTO
{
    public sealed record RenewalPeriodResponse(
        int Interval,
        string Type);
}
