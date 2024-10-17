using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Directories.DTO;
using TraffiLearn.Domain.Aggregates.Directories;
using TraffiLearn.Domain.Shared;
using Directory = TraffiLearn.Domain.Aggregates.Directories.Directory;

namespace TraffiLearn.Application.Directories.Queries.GetAll
{
    public sealed class GetAllDirectoriesQueryHandler
        : IRequestHandler<GetAllDirectoriesQuery, Result<IEnumerable<DirectoryResponse>>>
    {
        private readonly IDirectoryRepository _directoryRepository;
        private readonly Mapper<Directory, DirectoryResponse> _directoryMapper;

        public GetAllDirectoriesQueryHandler(
            IDirectoryRepository directoryRepository,
            Mapper<Directory, DirectoryResponse> directoryMapper)
        {
            _directoryRepository = directoryRepository;
            _directoryMapper = directoryMapper;
        }

        public async Task<Result<IEnumerable<DirectoryResponse>>> Handle(
            GetAllDirectoriesQuery request,
            CancellationToken cancellationToken)
        {
            var directories = await _directoryRepository.GetAllAsync(
                cancellationToken);

            return Result.Success(_directoryMapper.Map(directories));
        }
    }
}
