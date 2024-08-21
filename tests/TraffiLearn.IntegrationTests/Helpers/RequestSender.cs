using Azure.Core;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using TraffiLearn.IntegrationTests.Auth;

namespace TraffiLearn.IntegrationTests.Helpers
{
    internal sealed class RequestSender
    {
        private readonly HttpClient _httpClient;
        private readonly Authenticator _authenticator;
        private readonly Encoding _encoding = Encoding.UTF8;

        public RequestSender(
            HttpClient httpClient,
            Authenticator authenticator)
        {
            _httpClient = httpClient;
            _authenticator = authenticator;
        }

        public async Task<HttpResponseMessage> SendJsonRequestAsRegularUserWithTestCredentials<TValue>(
           HttpRequestMessage request,
           TValue value)
        {
            var loginResponse = await _authenticator.LoginAsRegularUserUsingTestCredentialsAsync();

            SetAuthorizationHeader(
                request,
                loginResponse.AccessToken);

            SetJsonContentHeader(
                request,
                value);

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> SendJsonRequestAsAdminWithTestCredentials<TValue>(
            HttpRequestMessage request,
            TValue value)
        {
            var loginResponse = await _authenticator.LoginAsAdminUsingTestCredentialsAsync();

            SetAuthorizationHeader(
                request,
                loginResponse.AccessToken);

            SetJsonContentHeader(
                request,
                value);

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> SendJsonRequestAsOwnerWithTestCredentials<TValue>(
            HttpRequestMessage request,
            TValue value)
        {
            var loginResponse = await _authenticator.LoginAsOwnerUsingTestCredentialsAsync();

            SetAuthorizationHeader(
                request,
                loginResponse.AccessToken);

            SetJsonContentHeader(
                request,
                value);

            return await _httpClient.SendAsync(request);
        }

        private void SetJsonContentHeader<TValue>(
            HttpRequestMessage request,
            TValue value)
        {
            request.Content = new StringContent(
                JsonSerializer.Serialize(value),
                encoding: _encoding,
                mediaType: MediaTypeNames.Application.Json);
        }

        public async Task<TValue> GetFromJsonAsRegularUserAsync<TValue>(
            string requestUri)
        {
            var httpRequestMessage = new HttpRequestMessage(
                method: HttpMethod.Get,
                requestUri: requestUri);

            var loginResponse = await _authenticator
                .LoginAsRegularUserUsingTestCredentialsAsync();

            SetAuthorizationHeader(
                httpRequestMessage, 
                accessToken: loginResponse.AccessToken);

            var response = await _httpClient.SendAsync(httpRequestMessage);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TValue>();

            if (result is null)
            {
                throw new JsonException("Failed to parse value from json.");
            }

            return result;
        }

        private void SetAuthorizationHeader(
            HttpRequestMessage request,
            string accessToken)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue(
                AuthConstants.Scheme,
                parameter: accessToken);
        }
    }
}
