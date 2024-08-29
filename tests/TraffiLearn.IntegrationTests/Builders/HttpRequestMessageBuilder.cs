using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace TraffiLearn.IntegrationTests.Builders
{
    public sealed class HttpRequestMessageBuilder
    {
        private readonly HttpRequestMessage _httpRequestMessage;
        private readonly Encoding _encoding = Encoding.UTF8;

        public HttpRequestMessageBuilder(
            HttpMethod method,
            string requestUri)
        {
            _httpRequestMessage = new HttpRequestMessage(
                method,
                requestUri);
        }

        public HttpRequestMessageBuilder WithJsonContent<TValue>(TValue value)
        {
            _httpRequestMessage.Content = new StringContent(
                content: JsonSerializer.Serialize(value),
                encoding: _encoding,
                mediaType: MediaTypeNames.Application.Json);

            return this;
        }

        public HttpRequestMessageBuilder WithAuthorization(
            string scheme,
            string parameter)
        {
            _httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue(
                scheme,
                parameter);

            return this;
        }

        public HttpRequestMessageBuilder WithMultipartFormDataContent<TValue>(
            TValue value,
            IFormFile? file = null)
        {
            var content = new MultipartFormDataContent();

            var valueContent = new StringContent(
                content: JsonSerializer.Serialize(value),
                encoding: _encoding,
                mediaType: MediaTypeNames.Application.Json);

            valueContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                Name = "\"Request\""
            };

            content.Add(valueContent);

            if (file is not null)
            {
                var fileContent = new StreamContent(file.OpenReadStream());

                fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);

                var dispositionType = "form-data";

                fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue(dispositionType)
                {
                    Name = "\"Image\"",
                    FileName = $"\"{file.FileName}\""
                };

                content.Add(fileContent);
            }

            _httpRequestMessage.Content = content;

            return this;
        }

        public HttpRequestMessage Build()
        {
            return _httpRequestMessage;
        }
    }
}
