using MediatR;
using Microsoft.Extensions.Logging;
using System.Transactions;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Errors;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Enums;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Users;

namespace TraffiLearn.Application.Commands.Auth.RegisterAdmin
{
    internal sealed class RegisterAdminCommandHandler
        : IRequestHandler<RegisterAdminCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUserContextService<Guid> _userContextService;
        private readonly Mapper<RegisterAdminCommand, Result<User>> _commandMapper;
        private readonly Mapper<User, ApplicationUser> _userMapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterAdminCommandHandler> _logger;

        public RegisterAdminCommandHandler(
            IUserRepository userRepository,
            IIdentityService<ApplicationUser> identityService,
            IUserContextService<Guid> userContextService,
            Mapper<RegisterAdminCommand, Result<User>> commandMapper,
            Mapper<User, ApplicationUser> userMapper,
            IUnitOfWork unitOfWork,
            ILogger<RegisterAdminCommandHandler> logger)
        {
            _userRepository = userRepository;
            _identityService = identityService;
            _userContextService = userContextService;
            _commandMapper = commandMapper;
            _userMapper = userMapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            RegisterAdminCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling RegisterAdminCommand");

            var mappingResult = _commandMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var callerId = _userContextService.FetchAuthenticatedUserId();

            _logger.LogInformation("Succesfully fetched caller id. Caller ID: {CallerId}", callerId);

            var caller = await _userRepository.GetByIdAsync(
                new UserId(callerId),
                cancellationToken);

            if (caller is null)
            {
                _logger.LogCritical(InternalErrors.AuthorizationFailure.Description);

                return InternalErrors.AuthorizationFailure;
            }

            if (IsNotAllowedToCreateAdmins(caller))
            {
                _logger.LogWarning("Caller tried to create an admin account not having enough permissions. Caller role: {role}", caller.Role.ToString());

                return UserErrors.NotAllowedToPerformAction;
            }

            var newUser = mappingResult.Value;

            var existsSameUser = await _userRepository.ExistsAsync(
                newUser.Username,
                newUser.Email,
                cancellationToken);

            if (existsSameUser)
            {
                return UserErrors.AlreadyRegistered;
            }

            var identityUser = _userMapper.Map(newUser);

            using (var transaction = new TransactionScope(
                TransactionScopeAsyncFlowOption.Enabled))
            {
                await _userRepository.AddAsync(newUser);

                await _identityService.CreateAsync(
                    identityUser, 
                    password: request.Password);

                await _identityService.AddToRoleAsync(
                    identityUser,
                    roleName: newUser.Role.ToString());

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                transaction.Complete();
            }

            _logger.LogInformation(
                "Succesfully registered a new admin. Username: {username}", 
                newUser.Username.Value);

            return Result.Success();
        }

        private bool IsNotAllowedToCreateAdmins(User user)
        {
            return user.Role < Role.Owner;
        }
    }
}
