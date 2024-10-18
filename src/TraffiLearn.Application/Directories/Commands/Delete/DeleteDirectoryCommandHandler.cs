using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Directories;
using TraffiLearn.Domain.Aggregates.Directories.Errors.Directories;
using TraffiLearn.Domain.Aggregates.Directories.ValueObjects.Directories;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Directories.Commands.Delete
{
    internal sealed class DeleteDirectoryCommandHandler
        : IRequestHandler<DeleteDirectoryCommand, Result>
    {
        private readonly IDirectoryRepository _directoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDirectoryCommandHandler(
            IDirectoryRepository directoryRepository,
            IUnitOfWork unitOfWork)
        {
            _directoryRepository = directoryRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DeleteDirectoryCommand request,
            CancellationToken cancellationToken)
        {
            var directory = await _directoryRepository.GetByIdAsync(
                directoryId: new DirectoryId(request.DirectoryId.Value),
                cancellationToken);

            if (directory is null)
            {
                return DirectoryErrors.NotFound;
            }

            await _directoryRepository.DeleteAsync(directory);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
