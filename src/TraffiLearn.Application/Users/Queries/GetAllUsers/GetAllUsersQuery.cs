using MediatR;
using TraffiLearn.Application.Users.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Queries.GetAllUsers
{
    public sealed class GetAllUsersQuery 
        : IRequest<Result<IEnumerable<UserResponse>>>;
}
