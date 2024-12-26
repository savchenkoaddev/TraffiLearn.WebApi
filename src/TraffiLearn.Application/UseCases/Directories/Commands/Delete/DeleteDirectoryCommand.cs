using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Directories.Commands.Delete
{
    public sealed record DeleteDirectoryCommand(
        Guid DirectoryId) : IRequest<Result>;
}
