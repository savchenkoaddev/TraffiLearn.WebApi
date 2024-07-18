namespace TraffiLearn.Application.DTO.Blobs.Response
{
    public sealed record BlobResponse(
        Stream Stream,
        string ContentType);
}
