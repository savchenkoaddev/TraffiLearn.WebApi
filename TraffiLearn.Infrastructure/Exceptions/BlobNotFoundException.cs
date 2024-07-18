namespace TraffiLearn.Infrastructure.Exceptions
{
    public sealed class BlobNotFoundException : Exception
    {
        public BlobNotFoundException(string blobName) : base($"Blob with {blobName} name has not been found.")
        { }
    }
}
