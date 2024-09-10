using MediatR;
using TraffiLearn.Application.Users.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Users.Queries.GetAllAdmins
{
    public sealed record GetAllAdminsQuery 
        : IRequest<Result<IEnumerable<UserResponse>>>;
}
