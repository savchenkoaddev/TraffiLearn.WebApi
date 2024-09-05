using Microsoft.AspNetCore.Http;
using System.Text;

namespace TraffiLearn.Testing.Shared.Factories
{
    public static class ImageFixtureFactory
    {
        public static IFormFile CreateImage(
            string fileName = "testImage.jpg",
            string contentType = "image/jpeg",
            string content = "FakeImageContent")
        {
            var byteArray = Encoding.UTF8.GetBytes(content);

            var stream = new MemoryStream(byteArray);

            var formFile = new FormFile(stream, 0, stream.Length, "image", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

            return formFile;
        }
    }
}
