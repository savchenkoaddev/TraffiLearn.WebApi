using MediatR;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Exceptions;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Users;
using TraffiLearn.Domain.Users.Emails;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Auth.Commands.SendChangeEmailMessage
{
    internal sealed class SendChangeEmailMessageCommandHandler
        : IRequestHandler<SendChangeEmailMessageCommand, Result>
    {
        private readonly IUserContextService<Guid> _userContextService;
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IUserRepository _userRepository;
        private readonly IEmailService _emailService;

        public SendChangeEmailMessageCommandHandler(
            IUserContextService<Guid> userContextService,
            IIdentityService<ApplicationUser> identityService,
            IUserRepository userRepository,
            IEmailService emailService)
        {
            _userContextService = userContextService;
            _identityService = identityService;
            _userRepository = userRepository;
            _emailService = emailService;
        }

        public async Task<Result> Handle(
            SendChangeEmailMessageCommand request,
            CancellationToken cancellationToken)
        {
            var callerUserId = _userContextService.GetAuthenticatedUserId();

            var userId = new UserId(callerUserId);

            var newEmailResult = Email.Create(request.NewEmail);

            if (newEmailResult.IsFailure)
            {
                return newEmailResult.Error;
            }

            var newEmailIsTaken = await _userRepository.GetByEmailAsync(
                newEmailResult.Value, cancellationToken) is not null;

            if (newEmailIsTaken)
            {
                return UserErrors.EmailAlreadyTaken;
            }

            var user = await _userRepository.GetByIdAsync(
                userId, cancellationToken);

            if (user is null)
            {
                throw new AuthorizationFailureException();
            }

            var identityUser = await _identityService.GetByEmailAsync(
                user.Email);

            if (identityUser is null)
            {
                throw new DataInconsistencyException();
            }

            await _emailService.SendChangeEmailMessageAsync(
                newEmail: request.NewEmail,
                userId: callerUserId.ToString(),
                identityUser: identityUser);

            return Result.Success();
        }
    }
}
