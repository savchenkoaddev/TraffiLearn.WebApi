using MediatR;
using TraffiLearn.Application.Abstractions.Emails;
using TraffiLearn.Application.Abstractions.Identity;
using TraffiLearn.Application.Users.Identity;
using TraffiLearn.Domain.Users;
using TraffiLearn.Domain.Users.Emails;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Auth.Commands.SendRecoverPasswordMessage
{
    internal sealed class SendRecoverPasswordMessageCommandHandler
        : IRequestHandler<SendRecoverPasswordMessageCommand, Result>
    {
        private readonly IIdentityService<ApplicationUser> _identityService;
        private readonly IEmailService _emailService;

        public SendRecoverPasswordMessageCommandHandler(
            IIdentityService<ApplicationUser> identityService,
            IEmailService emailService)
        {
            _identityService = identityService;
            _emailService = emailService;
        }

        public async Task<Result> Handle(
            SendRecoverPasswordMessageCommand request,
            CancellationToken cancellationToken)
        {
            var identityUser = await _identityService.GetByEmailAsync(
                Email.Create(request.Email).Value);

            if (identityUser is null)
            {
                return UserErrors.NotFound;
            }

            if (!identityUser.EmailConfirmed)
            {
                return UserErrors.EmailNotConfirmed;
            }

            await _emailService.SendRecoverPasswordEmail(
                recipientEmail: request.Email,
                userId: identityUser.Id,
                identityUser: identityUser);

            return Result.Success();
        }
    }
}
