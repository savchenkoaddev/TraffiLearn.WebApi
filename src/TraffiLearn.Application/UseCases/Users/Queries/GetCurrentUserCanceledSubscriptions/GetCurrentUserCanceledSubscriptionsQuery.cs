using MediatR;
using TraffiLearn.Application.UseCases.CanceledSubscriptions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetCurrentUserCanceledSubscriptions
{
    public sealed record GetCurrentUserCanceledSubscriptionsQuery
        : IRequest<Result<IEnumerable<CanceledSubscriptionResponse>>>;
}
