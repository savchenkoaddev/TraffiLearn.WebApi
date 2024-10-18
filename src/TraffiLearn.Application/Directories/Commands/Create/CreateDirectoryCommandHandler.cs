using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Aggregates.Directories;
using TraffiLearn.Domain.Shared;
using Directory = TraffiLearn.Domain.Aggregates.Directories.Directory;

namespace TraffiLearn.Application.Directories.Commands.Create
{
    internal sealed class CreateDirectoryCommandHandler
        : IRequestHandler<CreateDirectoryCommand, Result<Guid>>
    {
        private readonly IDirectoryRepository _directoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mapper<CreateDirectoryCommand, Result<Directory>> _directoryMapper;

        public CreateDirectoryCommandHandler(
            IDirectoryRepository directoryRepository,
            IUnitOfWork unitOfWork,
            Mapper<CreateDirectoryCommand, Result<Directory>> directoryMapper)
        {
            _directoryRepository = directoryRepository;
            _unitOfWork = unitOfWork;
            _directoryMapper = directoryMapper;
        }

        public async Task<Result<Guid>> Handle(
            CreateDirectoryCommand request,
            CancellationToken cancellationToken)
        {
            var mappingResult = _directoryMapper.Map(request);

            if (mappingResult.IsFailure)
            {
                return Result.Failure<Guid>(mappingResult.Error);
            }

            var directory = mappingResult.Value;

            await _directoryRepository.InsertAsync(
                directory: directory,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success(directory.Id.Value);
        }
    }
}
