using MediatR;
using Microsoft.Extensions.Logging;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.Users.DTO;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Queries.GetAllAdmins
{
    internal sealed class GetAllAdminsQueryHandler
        : IRequestHandler<GetAllAdminsQuery, Result<IEnumerable<UserResponse>>>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IUserRepository _userRepository;
        private readonly Mapper<User, UserResponse> _userMapper;
        private readonly ILogger<GetAllAdminsQueryHandler> _logger;

        public GetAllAdminsQueryHandler(
            IUserContextService<Guid> userContextService,
            IUserRepository userRepository,
            Mapper<User, UserResponse> userMapper,
            ILogger<GetAllAdminsQueryHandler> logger)
        {
            _userContextService = userContextService;
            _userRepository = userRepository;
            _userMapper = userMapper;
            _logger = logger;
        }

        public async Task<Result<IEnumerable<UserResponse>>> Handle(
            GetAllAdminsQuery request, 
            CancellationToken cancellationToken)
        {
            var callerId = _userContextService.GetAuthenticatedUserId();

            var caller = await _userRepository.GetByIdAsync(
                userId: new UserId(callerId),
                cancellationToken);

            if (caller is null)
            {
                throw new AuthorizationFailureException();
            }

            _logger.LogInformation(
                "Succesfully fetched caller with the id: {callerId}", callerId);

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
