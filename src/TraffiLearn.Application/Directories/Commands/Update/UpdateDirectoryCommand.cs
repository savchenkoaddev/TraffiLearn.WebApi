using MediatR;
using TraffiLearn.Application.Directories.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Directories.Commands.Update
{
    public sealed record UpdateDirectoryCommand(
        Guid? Id,
        string? Name,
        IEnumerable<DirectorySectionRequest>? Sections) : IRequest<Result>;
}
