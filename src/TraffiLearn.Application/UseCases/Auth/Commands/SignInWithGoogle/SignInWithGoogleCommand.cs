using MediatR;
using TraffiLearn.Application.UseCases.Auth.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Auth.Commands.SignInWithGoogle
{
    public sealed record SignInWithGoogleCommand(
        string GoogleIdToken,
        string FirstTimeSignInPassword) : IRequest<Result<LoginResponse>>;
}
