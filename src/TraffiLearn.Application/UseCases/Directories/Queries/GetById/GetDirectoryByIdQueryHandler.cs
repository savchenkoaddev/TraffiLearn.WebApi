using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Directories.DTO;
using TraffiLearn.Domain.Directories;
using TraffiLearn.SharedKernel.Shared;
using Directory = TraffiLearn.Domain.Directories.Directory;

namespace TraffiLearn.Application.UseCases.Directories.Queries.GetById
{
    internal sealed class GetDirectoryByIdQueryHandler
        : IRequestHandler<GetDirectoryByIdQuery, Result<DirectoryResponse>>
    {
        private readonly IDirectoryRepository _directoryRepository;
        private readonly Mapper<Directory, DirectoryResponse> _directoryMapper;

        public GetDirectoryByIdQueryHandler(
            IDirectoryRepository directoryRepository,
            Mapper<Directory, DirectoryResponse> directoryMapper)
        {
            _directoryRepository = directoryRepository;
            _directoryMapper = directoryMapper;
        }

        public async Task<Result<DirectoryResponse>> Handle(
            GetDirectoryByIdQuery request,
            CancellationToken cancellationToken)
        {
            var directory = await _directoryRepository.GetByIdAsync(
                directoryId: new(request.DirectoryId.Value),
                cancellationToken);

            if (directory is null)
            {
                return Result.Failure<DirectoryResponse>(
                    DirectoryErrors.NotFound);
            }

            return _directoryMapper.Map(directory);
        }
    }
}
