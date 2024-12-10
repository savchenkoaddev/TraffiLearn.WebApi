using MediatR;
using TraffiLearn.Application.UseCases.Users.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetCurrentUserInfo
{
    public sealed record GetCurrentUserInfoQuery
        : IRequest<Result<CurrentUserResponse>>;
}
