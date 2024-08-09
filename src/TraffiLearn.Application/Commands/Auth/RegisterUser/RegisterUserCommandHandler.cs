using MediatR;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Commands.Auth.RegisterUser
{
    internal sealed class RegisterUserCommandHandler
        : IRequestHandler<RegisterUserCommand, Result>
    {
        private readonly IUserManagementService _userManagementService;
        private readonly Mapper<RegisterUserCommand, Result<User>> _commandMapper;

        public RegisterUserCommandHandler(
            IUserManagementService userManagementService,
            Mapper<RegisterUserCommand, Result<User>> commandMapper)
        {
            _userManagementService = userManagementService;
            _commandMapper = commandMapper;
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

            var createResult = await _userManagementService.CreateUserAsync(
                newUser,
                password: request.Password,
                cancellationToken);

            if (createResult.IsFailure)
            {
                return createResult.Error;
            }

            return Result.Success();
        }
    }
}
