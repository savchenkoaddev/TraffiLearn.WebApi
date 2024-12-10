using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.UseCases.Users.DTO;
using TraffiLearn.Domain.Users;
using TraffiLearn.Domain.Users.Roles;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetAllAdmins
{
    internal sealed class GetAllAdminsQueryHandler
        : IRequestHandler<GetAllAdminsQuery, Result<IEnumerable<UserResponse>>>
    {
        private readonly IAuthenticatedUserService _authenticatedUserService;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<User, UserResponse> _userMapper;
        private readonly ILogger<GetAllAdminsQueryHandler> _logger;

        public GetAllAdminsQueryHandler(
            IAuthenticatedUserService authenticatedUserService,
            IUserRepository userRepository,
            Mapper<User, UserResponse> userMapper,
            ILogger<GetAllAdminsQueryHandler> logger)
        {
            _authenticatedUserService = authenticatedUserService;
            _userRepository = userRepository;
            _userMapper = userMapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<UserResponse>>> Handle(
            GetAllAdminsQuery request,
            CancellationToken cancellationToken)
        {
            var caller = await _authenticatedUserService.GetAuthenticatedUserAsync(
                cancellationToken);

            if (CallerIsNotEligibleToGetAllAdmins(caller))
            {
                _logger.LogInformation(
                    "Caller with the role {role} tried to fetch all admins, " +
                    "but does not have enough rights. Returning error.", caller.Role);

                return Result.Failure<IEnumerable<UserResponse>>(
                    UserErrors.NotAllowedToPerformAction);
            }

            var allAdmins = await _userRepository.GetAllAdminsAsync(
                cancellationToken);

            _logger.LogInformation("Succesfully fetched all admins for the caller. Returning result.");

            return Result.Success(_userMapper.Map(allAdmins));
        }

        private bool CallerIsNotEligibleToGetAllAdmins(User caller)
        {
            return caller.Role < Role.Owner;
        }
    }
}
