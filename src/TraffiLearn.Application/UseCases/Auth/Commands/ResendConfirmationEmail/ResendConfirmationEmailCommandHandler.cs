using MediatR;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.UseCases.Users.Identity;
using TraffiLearn.Domain.Users;
using TraffiLearn.Domain.Users.Emails;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Auth.Commands.ResendConfirmationEmail
{
    internal sealed class ResendConfirmationEmailCommandHandler
        : IRequestHandler<ResendConfirmationEmailCommand, Result>
    {
        private readonly IUserRepository _userRepository;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IEmailService _emailService;

        public ResendConfirmationEmailCommandHandler(
            IUserRepository userRepository,
            IIdentityService<ApplicationUser> identityService,
            IEmailService emailService)
        {
            _userRepository = userRepository;
            _identityService = identityService;
            _emailService = emailService;
        }

        public async Task<Result> Handle(
            ResendConfirmationEmailCommand request,
            CancellationToken cancellationToken)
        {
            var emailResult = Email.Create(request.Email);

            if (emailResult.IsFailure)
            {
                return emailResult.Error;
            }

            var email = emailResult.Value;

            var user = await _userRepository.GetByEmailAsync(
                email, cancellationToken);

            if (user is null)
            {
                return UserErrors.NotFound;
            }

            if (user.IsEmailConfirmed)
            {
                return UserErrors.EmailAlreadyConfirmed;
            }

            var identityUser = await _identityService.GetByEmailAsync(email);

            if (identityUser is null)
            {
                throw new DataInconsistencyException();
            }

            await _emailService.PublishConfirmationEmailAsync(
                recipientEmail: email.Value,
                userId: identityUser.Id,
                identityUser: identityUser);

            return Result.Success();
        }
    }
}
