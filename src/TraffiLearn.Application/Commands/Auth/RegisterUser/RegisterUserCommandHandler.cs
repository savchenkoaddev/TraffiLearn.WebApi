using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly Mapper<RegisterUserCommand, Result<User>> _commandMapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<RegisterUserCommandHandler> _logger;

        public RegisterUserCommandHandler(
            IUserRepository userRepository,
            UserManager<ApplicationUser> userManager,
            Mapper<RegisterUserCommand, Result<User>> commandMapper,
            IUnitOfWork unitOfWork,
            ILogger<RegisterUserCommandHandler> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
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

            var identityUser = await _userManager.FindByEmailAsync(request.Email);

            if (identityUser is not null)
            {
                return UserErrors.AlreadyRegistered;
            }

            var user = mappingResult.Value;

            await _userRepository.AddAsync(
                user,
                cancellationToken);

            identityUser = CreateIdentityUser(user);

            var addResult = await AddIdentityUser(
                identityUser,
                password: request.Password);

            if (addResult.IsFailure)
            {
                return addResult.Error;
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Succesfully created a new user with {Email} email.", identityUser.Email);

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

        private async Task<Result> AddIdentityUser(
            ApplicationUser identityUser,
            string password)
        {
            var result = await _userManager.CreateAsync(
                identityUser,
                password);

            if (!result.Succeeded)
            {
                _logger.LogCritical("Failed to create identity user.");

                return Error.InternalFailure();
            }

            return Result.Success();
        }
    }
}
