namespace TraffiLearn.Application.DTO.Blobs.Response
{
    public sealed record DownloadBlobResponse(
        Stream Stream,
        string ContentType);
}
