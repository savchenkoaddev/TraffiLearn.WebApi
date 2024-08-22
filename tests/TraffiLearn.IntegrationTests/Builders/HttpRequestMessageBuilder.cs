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

        public HttpRequestMessage Build()
        {
            return _httpRequestMessage;
        }
    }
}
