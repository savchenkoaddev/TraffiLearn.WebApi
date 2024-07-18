namespace TraffiLearn.Application.DTO.Blobs.Response
{
    public sealed record UploadBlobResponse
    {
        public UploadBlobResponse(
            Uri containerUri,
            string blobName)
        {
            ContainerUri = containerUri;
            BlobName = blobName;
        }

        public Uri ContainerUri { get; init; }

        public string BlobName { get; init; }

        public string BlobUri => string.Concat(ContainerUri, "/", BlobName);
    }
}
