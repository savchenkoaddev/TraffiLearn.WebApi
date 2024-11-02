using MediatR;
using TraffiLearn.Application.Directories.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Directories.Queries.GetById
{
    public sealed record GetDirectoryByIdQuery(
        Guid? DirectoryId) : IRequest<Result<DirectoryResponse>>;
}
