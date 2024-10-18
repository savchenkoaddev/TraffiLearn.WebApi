using MediatR;
using TraffiLearn.Application.Directories.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Directories.Commands.Create
{
    public sealed record CreateDirectoryCommand(
        string? Name,
        IEnumerable<DirectorySectionRequest>? Sections) : IRequest<Result<Guid>>;
}
