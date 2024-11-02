using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Users.DTO;
using TraffiLearn.Domain.Users;
using TraffiLearn.Domain.Users.Roles;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Queries.GetAllUsers
{
    internal sealed class GetAllUsersQueryHandler
        : IRequestHandler<GetAllUsersQuery, Result<IEnumerable<UserResponse>>>
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<User, UserResponse> _userMapper;
        private readonly ILogger<GetAllUsersQueryHandler> _logger;

        public GetAllUsersQueryHandler(
            IAuthenticatedUserService authenticatedUserService,
            IUserRepository userRepository,
            Mapper<User, UserResponse> userMapper,
            ILogger<GetAllUsersQueryHandler> logger)
        {
            _authenticatedUserService = authenticatedUserService;
            _userRepository = userRepository;
            _userMapper = userMapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<UserResponse>>> Handle(
            GetAllUsersQuery request,
            CancellationToken cancellationToken)
        {
            var caller = await _authenticatedUserService.GetAuthenticatedUserAsync(
                cancellationToken);

            if (CallerIsNotEligibleToGetAllUsers(caller))
            {
                _logger.LogInformation(
                    "Caller with the role {role} tried to fetch all users, " +
                    "but does not have enough rights. Returning error.", caller.Role);

                return Result.Failure<IEnumerable<UserResponse>>(
                    UserErrors.NotAllowedToPerformAction);
            }

            var allUsers = await _userRepository.GetAllAsync(
                cancellationToken);

            _logger.LogInformation("Succesfully fetched all users for the caller. Returning result.");

            return Result.Success(_userMapper.Map(allUsers));
        }

        private static bool CallerIsNotEligibleToGetAllUsers(User caller)
        {
            return caller.Role < Role.Admin;
        }
    }
}
