using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Auth.Commands.RegisterUser
{
    public sealed record RegisterUserCommand(
        string Username,
        string Email,
        string Password) : IRequest<Result>;
}
