using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Errors;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.ConfirmChangeEmail
{
    internal sealed class ConfirmChangeEmailCommandHandler
        : IRequestHandler<ConfirmChangeEmailCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUnitOfWork _unitOfWork;

        public ConfirmChangeEmailCommandHandler(
            IUserRepository userRepository, 
            IIdentityService<ApplicationUser> identityService, 
            IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _identityService = identityService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            ConfirmChangeEmailCommand request, 
            CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(
                new UserId(request.UserId.Value),
                cancellationToken);

            if (user is null)
            {
                return UserErrors.NotFound;
            }

            var email = user.Email;

            var identityUser = await _identityService.GetByEmailAsync(
                email);

            if (identityUser is null)
            {
                throw new DataInconsistencyException();
            }

            var result = await _identityService.ChangeEmailAsync(
                identityUser,
                newEmail: request.NewEmail,
                token: request.Token);

            if (result.IsFailure)
            {
                return result.Error;
            }

            var newEmailResult = Email.Create(request.NewEmail);

            if (newEmailResult.IsFailure)
            {
                return newEmailResult.Error;
            }

            var newEmail = newEmailResult.Value;

            user.ChangeEmail(newEmail);

            await _unitOfWork.SaveChangesAsync();

            return Result.Success();
        }
    }
}
