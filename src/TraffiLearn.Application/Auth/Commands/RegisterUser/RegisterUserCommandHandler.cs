using MediatR;
using Microsoft.Extensions.Logging;
using System.Transactions;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.RegisterUser
{
    internal sealed class RegisterUserCommandHandler
        : IRequestHandler<RegisterUserCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly Mapper<RegisterUserCommand, Result<User>> _commandMapper;
        private readonly Mapper<User, ApplicationUser> _userMapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            IIdentityService<ApplicationUser> identityService,
            Mapper<RegisterUserCommand, Result<User>> commandMapper,
            Mapper<User, ApplicationUser> userMapper,
            IUnitOfWork unitOfWork,
            ILogger<RegisterUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _identityService = identityService;
            _commandMapper = commandMapper;
            _userMapper = userMapper;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result> Handle(
            RegisterUserCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling RegisterAdminCommand");

            var mappingResult = _commandMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
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
                await _userRepository.InsertAsync(
                    newUser,
                    cancellationToken);

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
                "Succesfully registered a new user. Username: {username}",
                newUser.Username.Value);

            return Result.Success();
        }
    }
}
