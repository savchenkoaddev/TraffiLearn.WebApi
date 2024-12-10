using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.UseCases.Users.DTO;
using TraffiLearn.Domain.Users;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetCurrentUserInfo
{
    internal sealed class GetCurrentUserInfoQueryHandler
        : IRequestHandler<GetCurrentUserInfoQuery, Result<CurrentUserResponse>>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<User, CurrentUserResponse> _userMapper;

        public GetCurrentUserInfoQueryHandler(
            IUserContextService<Guid> userContextService,
            IUserRepository userRepository,
            Mapper<User, CurrentUserResponse> userMapper)
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
            _userMapper = userMapper;
        }

        public async Task<Result<CurrentUserResponse>> Handle(
            GetCurrentUserInfoQuery request,
            CancellationToken cancellationToken)
        {
            var userId = new UserId(_userContextService
                .GetAuthenticatedUserId());

            var user = await _userRepository.GetByIdWithPlanAsync(
                userId, cancellationToken);

            if (user is null)
            {
                throw new InvalidOperationException("Authenticated user is not found.");
            }

            return _userMapper.Map(user);
        }
    }
}
