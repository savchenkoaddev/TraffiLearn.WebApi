using MediatR;
using TraffiLearn.Application.Common.DTO;
using TraffiLearn.Application.UseCases.SubscriptionPlans.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.SubscriptionPlans.Commands.Create
{
    public sealed record CreateSubscriptionPlanCommand(
        string? Tier,
        string? Description,
        PriceRequest? Price,
        RenewalPeriodRequest? RenewalPeriod,
        List<string>? Features) : IRequest<Result<Guid>>;
}
