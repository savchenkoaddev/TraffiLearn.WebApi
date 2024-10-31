using MediatR;
using TraffiLearn.Application.Directories.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Directories.Queries.GetAll
{
    public sealed record GetAllDirectoriesQuery
        : IRequest<Result<IEnumerable<DirectoryResponse>>>;
}
