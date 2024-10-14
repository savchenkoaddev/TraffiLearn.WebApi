using Microsoft.AspNetCore.Http;
using TraffiLearn.Domain.Aggregates.Common.ImageUri;

namespace TraffiLearn.Application.Abstractions.Storage
{
    public interface IImageService
    {
        Task<ImageUri> UploadImageAsync(
            IFormFile image,
            CancellationToken cancellationToken = default);

        Task DeleteAsync(
            ImageUri imageUri,
            CancellationToken cancellationToken = default);
    }
}
