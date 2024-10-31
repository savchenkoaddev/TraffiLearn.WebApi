using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TraffiLearn.Domain.Aggregates.Users.Roles;
using TraffiLearn.IntegrationTests.Auth;
using TraffiLearn.IntegrationTests.Builders;
using TraffiLearn.IntegrationTests.Extensions;

namespace TraffiLearn.IntegrationTests.Helpers
{
    public sealed class RequestSender
    {
        private readonly HttpClient _httpClient;
        private readonly Authenticator _authenticator;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly Dictionary<Role, RoleCredentials> _roleCredentials;
        private readonly IMemoryCache _cache;

        public RequestSender(
            HttpClient httpClient,
            Authenticator authenticator,
            IMemoryCache cache)
        {
            _httpClient = httpClient;
            _authenticator = authenticator;

            _roleCredentials = CreateRoleCredentialsDictionary();
            _cache = cache;
        }

        public async Task<HttpResponseMessage> SendJsonAsync<TValue>(
            HttpMethod method,
            string requestUri,
            TValue value,
            Role? sentFromRole = null)
        {
            var builder = new HttpRequestMessageBuilder(
                    method,
                    requestUri)
                .WithJsonContent(value);

            var request = await BuildHttpRequestWithOptionalAuthorizationAsync(
                builder,
                sentFromRole);

            return await _httpClient.SendAsync(request);
        }

        public async Task<TResponse> SendAndGetJsonAsync<TRequest, TResponse>(
           HttpMethod method,
           string requestUri,
           TRequest value,
           Role? sentFromRole = null)
        {
            var builder = new HttpRequestMessageBuilder(
                    method,
                    requestUri)
                .WithJsonContent(value);

            var request = await BuildHttpRequestWithOptionalAuthorizationAsync(
                builder,
                sentFromRole);

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadFromJsonAsync<TResponse>();

            if (content is null)
            {
                throw new InvalidOperationException(nameof(content));
            }

            return content;
        }

        public async Task<HttpResponseMessage> DeleteAsync(
            string requestUri,
            Role? deletedWithRole = null)
        {
            var builder = new HttpRequestMessageBuilder(
                HttpMethod.Delete,
                requestUri);

            var request = await BuildHttpRequestWithOptionalAuthorizationAsync(
                builder,
                deletedWithRole);

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> PutAsJsonAsync<TRequest>(
            string requestUri,
            TRequest request,
            Role? putWithRole = null)
        {
            var builder = new HttpRequestMessageBuilder(
                    HttpMethod.Put,
                    requestUri)
                .WithJsonContent(request);

            var httpRequest = await BuildHttpRequestWithOptionalAuthorizationAsync(
                builder,
                putWithRole);

            return await _httpClient.SendAsync(httpRequest);
        }


        public async Task<HttpResponseMessage> PutAsync(
            string requestUri,
            Role? putWithRole = null)
        {
            var builder = new HttpRequestMessageBuilder(
                    HttpMethod.Put,
                    requestUri);

            var httpRequest = await BuildHttpRequestWithOptionalAuthorizationAsync(
                builder,
                putWithRole);

            return await _httpClient.SendAsync(httpRequest);
        }


        public async Task<HttpResponseMessage> GetAsync(
            string requestUri,
            Role? getWithRole = null)
        {
            var builder = new HttpRequestMessageBuilder(
                HttpMethod.Get,
                requestUri);

            var request = await BuildHttpRequestWithOptionalAuthorizationAsync(
                builder,
                getWithRole);

            return await _httpClient.SendAsync(request);
        }

        public async Task<TResponse> GetFromJsonAsync<TResponse>(
            string requestUri,
            Role? getWithRole = null)
        {
            var builder = new HttpRequestMessageBuilder(
                HttpMethod.Get,
                requestUri);

            var request = await BuildHttpRequestWithOptionalAuthorizationAsync(
                builder,
                getWithRole);

            return await SendAndParseJsonResponseAsync<TResponse>(request);
        }

        public async Task<HttpResponseMessage> SendMultipartFormDataWithJsonAndFileRequest<TValue>(
            HttpMethod method,
            string requestUri,
            TValue value,
            IFormFile? file = null,
            Role? sentFromRole = null)
        {
            var builder = new HttpRequestMessageBuilder(
                    method,
                    requestUri)
                .WithMultipartFormDataContent(
                    value,
                    file);

            var request = await BuildHttpRequestWithOptionalAuthorizationAsync(
                builder,
                sentFromRole);

            return await _httpClient.SendAsync(request);
        }

        public async Task EnsureEachSentJsonRequestReturnsStatusCodeAsync<TRequest>(
            HttpMethod method,
            string requestUri,
            IEnumerable<TRequest> requests,
            HttpStatusCode statusCode,
            Role? sentFromRole = null)
        {
            var responses = await SendAllAsJsonAsync(
                method,
                requestUri,
                requests,
                sentFromRole);

            foreach (var request in responses)
            {
                request.AssertStatusCode(statusCode);
            }
        }

        public Task EnsureEachSentJsonRequestReturnsBadRequestAsync<TRequest>(
            HttpMethod method,
            string requestUri,
            IEnumerable<TRequest> requests,
            Role? sentFromRole = null)
        {
            return EnsureEachSentJsonRequestReturnsStatusCodeAsync(
                method,
                requestUri,
                requests,
                statusCode: HttpStatusCode.BadRequest,
                sentFromRole);
        }

        public async Task<IEnumerable<HttpResponseMessage>> SendAllAsJsonAsync<TRequest>(
            HttpMethod method,
            string requestUri,
            IEnumerable<TRequest> requests,
            Role? sentFromRole = null)
        {
            List<HttpResponseMessage> responses = [];

            foreach (var request in requests)
            {
                responses.Add(await SendJsonAsync(
                    method,
                    requestUri,
                    request,
                    sentFromRole));
            }

            return responses;
        }

        private async Task<HttpRequestMessage> BuildHttpRequestWithOptionalAuthorizationAsync(
            HttpRequestMessageBuilder builder,
            Role? role)
        {
            if (role is not null)
            {
                var accessToken = await GetAccessTokenForRoleAsync(role.Value);

                builder.WithAuthorization(
                    scheme: AuthConstants.Scheme,
                    parameter: accessToken);
            }

            return builder.Build();
        }

        private async Task<TResponse> SendAndParseJsonResponseAsync<TResponse>(
            HttpRequestMessage requestMessage)
        {
            var response = await _httpClient.SendAsync(requestMessage);

            response.EnsureSuccessStatusCode();

            var str = await response.Content.ReadAsStringAsync();
            var result = await response.Content.ReadFromJsonAsync<TResponse>();

            if (result is null)
            {
                throw new JsonException("Failed to parse value from JSON.");
            }

            return result;
        }

        private RoleCredentials GetCredentialsFromRole(Role role)
        {
            if (!_roleCredentials.TryGetValue(role, out var credentials))
            {
                throw new InvalidOperationException($"Invalid role '{role}' provided. Unable to send JSON request due to missing or unrecognized role credentials.");
            }

            return credentials;
        }

        private async Task<string> GetAccessTokenForRoleAsync(Role role)
        {
            if (_cache.TryGetAccessTokenForRole(
                    role,
                    out string? cachedToken))
            {
                if (!string.IsNullOrEmpty(cachedToken))
                {
                    return cachedToken;
                }
            }

            var credentials = GetCredentialsFromRole(role);

            var loginResponse = await _authenticator.LoginAsync(credentials);

            var token = loginResponse.AccessToken;

            _cache.SetAccessToken(
                role: role,
                accessToken: token);

            return token;
        }

        private Dictionary<Role, RoleCredentials> CreateRoleCredentialsDictionary()
        {
            return new Dictionary<Role, RoleCredentials>
            {
                { Role.RegularUser, AuthTestCredentials.RegularUser },
                { Role.Admin, AuthTestCredentials.Admin },
                { Role.Owner, AuthTestCredentials.Owner }
            };
        }
    }
}
