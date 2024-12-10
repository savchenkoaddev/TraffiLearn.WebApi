using MediatR;
using TraffiLearn.Application.UseCases.Directories.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Directories.Queries.GetAll
{
    public sealed record GetAllDirectoriesQuery
        : IRequest<Result<IEnumerable<DirectoryResponse>>>;
}
