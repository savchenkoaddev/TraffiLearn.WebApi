using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.UseCases.CanceledSubscriptions.DTO;
using TraffiLearn.Domain.Shared.CanceledSubscriptions;
using TraffiLearn.Domain.Users;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetCurrentUserCanceledSubscriptions
{
    internal sealed class GetCurrentUserCanceledSubscriptionsQueryHandler
        : IRequestHandler<GetCurrentUserCanceledSubscriptionsQuery,
            Result<IEnumerable<CanceledSubscriptionResponse>>>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly ICanceledSubscriptionRepository _repository;
        private readonly Mapper<CanceledSubscription, CanceledSubscriptionResponse> _mapper;

        public GetCurrentUserCanceledSubscriptionsQueryHandler(
            IUserContextService<Guid> userContextService,
            ICanceledSubscriptionRepository repository,
            Mapper<CanceledSubscription, CanceledSubscriptionResponse> mapper)
        {
            _userContextService = userContextService;
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<IEnumerable<CanceledSubscriptionResponse>>> Handle(
            GetCurrentUserCanceledSubscriptionsQuery request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService.GetAuthenticatedUserId());

            var canceledSubscriptions = await _repository
                .GetAllByUserIdWithSubscriptionsAsync(
                    userId, cancellationToken);

            return Result.Success(_mapper.Map(canceledSubscriptions));
        }
    }
}
