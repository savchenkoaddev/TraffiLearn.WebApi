using MediatR;
using Microsoft.Extensions.Logging;
using System.Transactions;
using TraffiLearn.Application.Abstractions.Auth;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Identity;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Errors.Users;
using TraffiLearn.Domain.RepositoryContracts;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Auth.RegisterUser
{
    internal sealed class RegisterUserCommandHandler
        : IRequestHandler<RegisterUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAuthService<ApplicationUser> _authService;
        private readonly Mapper<RegisterUserCommand, Result<User>> _commandMapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IAuthService<ApplicationUser> authService,
            Mapper<RegisterUserCommand, Result<User>> commandMapper,
            IUnitOfWork unitOfWork,
            ILogger<RegisterUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _authService = authService;
            _commandMapper = commandMapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            RegisterUserCommand request,
            CancellationToken cancellationToken)
        {
            var mappingResult = _commandMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var newUser = mappingResult.Value;

            var existsSameUser = await _userRepository
                .ExistsAsync(
                    newUser.Username,
                    newUser.Email,
                    cancellationToken);

            if (existsSameUser)
            {
                return UserErrors.AlreadyRegistered;
            }

            // Transaction is required due to features of UserManager.
            using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                await _userRepository.AddAsync(
                    newUser,
                    cancellationToken);

                var newIdentityUser = CreateIdentityUser(newUser);

                var addResult = await _authService.AddIdentityUser(
                    identityUser: newIdentityUser,
                    request.Password);

                if (addResult.IsFailure)
                {
                    return addResult.Error;
                }

                var roleAssignmentResult = await _authService.AssignRoleToUser(
                    newIdentityUser,
                    role: newUser.Role);

                if (roleAssignmentResult.IsFailure)
                {
                    return roleAssignmentResult.Error;
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Succesfully created a new user with {Email} email.", newUser.Email);

                transaction.Complete();
            }

            return Result.Success();
        }

        private ApplicationUser CreateIdentityUser(User user)
        {
            return new ApplicationUser()
            {
                Id = user.Id.ToString(),
                Email = user.Email.Value,
                UserName = user.Username.Value
            };
        }
    }
}
