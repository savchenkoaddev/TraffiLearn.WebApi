using TraffiLearn.Application.DTO.Blobs;

namespace TraffiLearn.Application.Abstractions.Storage
{
    public interface IBlobService
    {
        Task<UploadBlobResponse> UploadAsync(
            Stream stream,
            string contentType,
            CancellationToken cancellationToken = default);

        Task<DownloadBlobResponse> DownloadAsync(
            string blobName,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            string blobUri,
            CancellationToken cancellationToken = default);
    }
}
