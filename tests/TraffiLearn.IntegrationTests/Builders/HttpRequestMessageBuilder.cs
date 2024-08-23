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

        public MultipartFormDataContent WithMultipartFormDataContent<TValue>(
            TValue value)
        {
            var content = new MultipartFormDataContent();

            foreach (var property in typeof(TValue).GetProperties())
            {
                var propertyValue = property.GetValue(value);

                if (propertyValue is not null)
                {
                    if (propertyValue is IFormFile formFile)
                    {
                        var fileContent = new StreamContent(formFile.OpenReadStream());

                        fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse(
                            formFile.ContentType);

                        content.Add(
                            content: fileContent,
                            name: property.Name,
                            fileName: formFile.FileName);
                    }
                    else
                    {
                        content.Add(
                            content: new StringContent(propertyValue.ToString() ?? string.Empty, _encoding), property.Name);
                    }
                }
            }

            return content;
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

        public HttpRequestMessage Build()
        {
            return _httpRequestMessage;
        }
    }
}
