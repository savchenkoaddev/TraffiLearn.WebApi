using MediatR;
using TraffiLearn.Application.Users.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Queries.GetCurrentUserInfo
{
    public sealed record GetCurrentUserInfoQuery
        : IRequest<Result<CurrentUserResponse>>;
}
