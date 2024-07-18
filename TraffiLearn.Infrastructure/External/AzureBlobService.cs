using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Infrastructure.Exceptions;
using TraffiLearn.Infrastructure.Options;

namespace TraffiLearn.Infrastructure.External
{
    public sealed class AzureBlobService : IBlobService
    {
        private readonly BlobServiceClient _blobClient;
        private readonly AzureBlobStorageSettings _storageSettings;
        private readonly BlobContainerClient _containerClient;

        public AzureBlobService(
            BlobServiceClient blobClient, 
            IOptions<AzureBlobStorageSettings> storageSettings)
        {
            _blobClient = blobClient;
            _storageSettings = storageSettings.Value;

            _containerClient = _blobClient.GetBlobContainerClient(
                _storageSettings.ContainerName);
        }

        public async Task DeleteAsync(string fileName, CancellationToken cancellationToken = default)
        {
            var succeeded = await _containerClient.DeleteBlobIfExistsAsync(
                fileName,
                cancellationToken: cancellationToken);

            if (!succeeded)
            {
                throw new InvalidOperationException("Failed to delete blob from the storage.");
            }
        }

        public async Task<FileResponse> DownloadAsync(string fileName, CancellationToken cancellationToken = default)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(fileName);

            var blobExists = await blobClient.ExistsAsync(cancellationToken);

            if (!blobExists)
            {
                throw new BlobNotFoundException(fileName);
            }

            var response = await blobClient.DownloadContentAsync(cancellationToken);

            return new FileResponse(
                response.Value.Content.ToStream(),
                response.Value.Details.ContentType);
        }

        public async Task<string> UploadAsync(Stream stream, string contentType, CancellationToken cancellationToken = default)
        {
            var blobName = Guid.NewGuid()
                               .ToString();

            BlobClient blobClient = _containerClient.GetBlobClient(blobName);

            await blobClient.UploadAsync(
                stream,
                new BlobHttpHeaders
                {
                    ContentType = contentType
                },
                cancellationToken: cancellationToken);

            return blobName;
        }
    }
}
