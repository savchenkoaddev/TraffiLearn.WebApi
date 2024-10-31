using MediatR;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Directories.Commands.Delete
{
    public sealed record DeleteDirectoryCommand(
        Guid? DirectoryId) : IRequest<Result>;
}
