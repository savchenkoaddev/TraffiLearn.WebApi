using MediatR;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.SendChangeEmailMessage
{
    internal sealed class SendChangeEmailMessageCommandHandler
        : IRequestHandler<SendChangeEmailMessageCommand, Result>
    {
        public Task<Result> Handle(
            SendChangeEmailMessageCommand request, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
