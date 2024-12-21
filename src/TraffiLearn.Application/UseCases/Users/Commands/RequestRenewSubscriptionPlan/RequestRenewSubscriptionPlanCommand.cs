using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Commands.RequestRenewSubscriptionPlan
{
    public sealed record RequestRenewSubscriptionPlanCommand
        : IRequest<Result<Uri>>;
}
