using MediatR;
using TraffiLearn.Application.UseCases.Directories.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Directories.Commands.Create
{
    public sealed record CreateDirectoryCommand(
        string Name,
        IEnumerable<DirectorySectionRequest> Sections) : IRequest<Result<Guid>>;
}
