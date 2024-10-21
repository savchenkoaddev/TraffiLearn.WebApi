using MediatR;
using TraffiLearn.Application.Auth.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Auth.Commands.SignInWithGoogle
{
    internal sealed class SignInWithGoogleCommandHandler
        : IRequestHandler<SignInWithGoogleCommand, Result<LoginResponse>>
    {
        public Task<Result<LoginResponse>> Handle(
            SignInWithGoogleCommand request, 
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
