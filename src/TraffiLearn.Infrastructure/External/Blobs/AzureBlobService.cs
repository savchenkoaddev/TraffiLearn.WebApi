using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Application.Storage.Blobs.DTO;
using TraffiLearn.Infrastructure.Exceptions;
using TraffiLearn.Infrastructure.External.Blobs.Options;

namespace TraffiLearn.Infrastructure.External.Blobs
{
    internal sealed class AzureBlobService : IBlobService
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

        public async Task DeleteAsync(
            string blobUri, 
            CancellationToken cancellationToken = default)
        {
            var blobName = blobUri.Split('/', '\\').Last();

            var succeeded = await _containerClient.DeleteBlobIfExistsAsync(
                blobName,
                cancellationToken: cancellationToken);

            if (!succeeded)
            {
                throw new InvalidOperationException("Failed to delete blob from the storage.");
            }
        }

        public async Task<DownloadBlobResponse> DownloadAsync(
            string blobName, 
            CancellationToken cancellationToken = default)
        {
            BlobClient blobClient = _containerClient.GetBlobClient(blobName);

            var blobExists = await blobClient.ExistsAsync(cancellationToken);

            if (!blobExists)
            {
                throw new BlobNotFoundException(blobName);
            }

            var response = await blobClient.DownloadContentAsync(cancellationToken);

            return new DownloadBlobResponse(
                response.Value.Content.ToStream(),
                response.Value.Details.ContentType);
        }

        public async Task<UploadBlobResponse> UploadAsync(
            Stream stream, 
            string contentType, 
            CancellationToken cancellationToken = default)
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

            return new UploadBlobResponse(
                _containerClient.Uri,
                blobName);
        }
    }
}
