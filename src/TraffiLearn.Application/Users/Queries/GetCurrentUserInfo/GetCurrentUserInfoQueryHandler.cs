using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Users.DTO;
using TraffiLearn.Domain.Users;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Queries.GetCurrentUserInfo
{
    internal sealed class GetCurrentUserInfoQueryHandler
        : IRequestHandler<GetCurrentUserInfoQuery, Result<CurrentUserResponse>>
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly Mapper<User, CurrentUserResponse> _userMapper;

        public GetCurrentUserInfoQueryHandler(
            IAuthenticatedUserService authenticatedUserService,
            Mapper<User, CurrentUserResponse> userMapper)
        {
            _authenticatedUserService = authenticatedUserService;
            _userMapper = userMapper;
        }

        public async Task<Result<CurrentUserResponse>> Handle(
            GetCurrentUserInfoQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _authenticatedUserService
                .GetAuthenticatedUserAsync(cancellationToken);

            return _userMapper.Map(user);
        }
    }
}
