using MediatR;
using TraffiLearn.Application.UseCases.Users.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetAllAdmins
{
    public sealed record GetAllAdminsQuery
        : IRequest<Result<IEnumerable<UserResponse>>>;
}
