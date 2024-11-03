using MediatR;
using TraffiLearn.Application.Common.DTO;
using TraffiLearn.Application.SubscriptionPlans.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.SubscriptionPlans.Commands.Create
{
    public sealed record CreateSubscriptionPlanCommand(
        string? Tier,
        string? Description,
        PriceRequest? Price,
        RenewalPeriodRequest? RenewalPeriod,
        List<string>? Features) : IRequest<Result<Guid>>;
}
