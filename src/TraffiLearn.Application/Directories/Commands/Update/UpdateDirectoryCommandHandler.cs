using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Directories;
using TraffiLearn.Domain.Shared;
using Directory = TraffiLearn.Domain.Aggregates.Directories.Directory;

namespace TraffiLearn.Application.Directories.Commands.Update
{
    internal sealed class UpdateDirectoryCommandHandler
        : IRequestHandler<UpdateDirectoryCommand, Result>
    {
        private readonly IDirectoryRepository _directoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mapper<UpdateDirectoryCommand, Result<Directory>> _directoryMapper;

        public UpdateDirectoryCommandHandler(
            IDirectoryRepository directoryRepository,
            IUnitOfWork unitOfWork,
            Mapper<UpdateDirectoryCommand, Result<Directory>> directoryMapper)
        {
            _directoryRepository = directoryRepository;
            _unitOfWork = unitOfWork;
            _directoryMapper = directoryMapper;
        }

        public async Task<Result> Handle(
            UpdateDirectoryCommand request,
            CancellationToken cancellationToken)
        {
            var directory = await _directoryRepository.GetByIdAsync(
                directoryId: new(request.Id.Value),
                cancellationToken);

            if (directory is null)
            {
                return DirectoryErrors.NotFound;
            }

            var mappingResult = _directoryMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return mappingResult.Error;
            }

            var newDirectory = mappingResult.Value;

            var updateResult = directory.Update(
                newDirectory.Name,
                newDirectory.Sections.ToList());

            if (updateResult.IsFailure)
            {
                return updateResult.Error;
            }

            await _directoryRepository.UpdateAsync(directory);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
