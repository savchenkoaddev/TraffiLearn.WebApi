using MediatR;
using TraffiLearn.Application.Users.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Users.Queries.GetAllUsers
{
    public sealed record GetAllUsersQuery
        : IRequest<Result<IEnumerable<UserResponse>>>;
}
