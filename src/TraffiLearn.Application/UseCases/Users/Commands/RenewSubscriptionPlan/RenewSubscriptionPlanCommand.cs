using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Commands.RenewSubscriptionPlan
{
    public sealed record RenewSubscriptionPlanCommand
        : IRequest<Result<DateTime>>;
}
