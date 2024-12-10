using MediatR;
using TraffiLearn.Application.UseCases.Users.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetAllUsers
{
    public sealed record GetAllUsersQuery
        : IRequest<Result<IEnumerable<UserResponse>>>;
}
