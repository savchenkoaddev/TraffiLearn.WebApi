using MediatR;
using TraffiLearn.Application.UseCases.Directories.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Directories.Queries.GetById
{
    public sealed record GetDirectoryByIdQuery(
        Guid DirectoryId) : IRequest<Result<DirectoryResponse>>;
}
