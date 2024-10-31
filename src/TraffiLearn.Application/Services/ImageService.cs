using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Application.Images.Options;
using TraffiLearn.Domain.Common.ImageUris;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Services
{
    internal sealed class ImageService : IImageService
    {
        private readonly IBlobService _blobService;
        private readonly ImageSettings _imageSettings;

        public ImageService(
            IBlobService blobService,
            IOptions<ImageSettings> imageSettings)
        {
            _blobService = blobService;
            _imageSettings = imageSettings.Value;
        }

        public Task DeleteAsync(
            ImageUri imageUri,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(imageUri, nameof(imageUri));

            return _blobService.DeleteAsync(
                blobUri: imageUri.Value,
                cancellationToken);
        }

        public async Task<ImageUri> UploadImageAsync(
            IFormFile image,
            CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(image, nameof(image));

            if (IsInvalidContentType(image.ContentType))
            {
                throw new ArgumentException("Image content type is invalid.");
            }

            using Stream stream = image.OpenReadStream();

            var uploadResponse = await _blobService.UploadAsync(
                stream,
                contentType: image.ContentType,
                cancellationToken);

            Result<ImageUri> imageUriResult = ImageUri.Create(uploadResponse.BlobUri);

            if (imageUriResult.IsFailure)
            {
                throw new InvalidOperationException("Failed to create image uri from created image.");
            }

            return imageUriResult.Value;
        }

        private bool IsInvalidContentType(string contentType)
        {
            return !_imageSettings.AllowedContentTypes.Contains(contentType);
        }
    }
}
