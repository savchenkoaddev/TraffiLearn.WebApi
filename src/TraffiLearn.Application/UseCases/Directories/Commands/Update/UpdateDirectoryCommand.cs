using MediatR;
using TraffiLearn.Application.UseCases.Directories.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Directories.Commands.Update
{
    public sealed record UpdateDirectoryCommand(
        Guid? Id,
        string? Name,
        IEnumerable<DirectorySectionRequest>? Sections) : IRequest<Result>;
}
