using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TraffiLearn.Domain.Aggregates.Users.Enums;
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

        public async Task<HttpResponseMessage> SendJsonRequestWithRole<TValue>(
            Role role,
            HttpMethod method,
            string requestUri,
            TValue value)
        {
            var accessToken = await GetAccessTokenForRoleAsync(role);

            var request = new HttpRequestMessageBuilder(method, requestUri)
                .WithJsonContent(value)
                .WithAuthorization(
                    scheme: AuthConstants.Scheme,
                    parameter: accessToken)
                .Build();

            return await _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> SendJsonRequest<TValue>(
            HttpMethod method,
            string requestUri,
            TValue value)
        {
            var request = new HttpRequestMessageBuilder(method, requestUri)
                .WithJsonContent(value)
                .Build();

            return await _httpClient.SendAsync(request);
        }

        public Task<HttpResponseMessage> DeleteAsync(
            string requestUri)
        {
            var request = new HttpRequestMessageBuilder(
                method: HttpMethod.Delete,
                requestUri)
                .Build();

            return _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> DeleteWithRoleAsync(
            Role role,
            string requestUri)
        {
            var accessToken = await GetAccessTokenForRoleAsync(role);

            var request = new HttpRequestMessageBuilder(
                method: HttpMethod.Delete,
                requestUri)
                .WithAuthorization(
                    scheme: AuthConstants.Scheme,
                    parameter: accessToken)
                .Build();

            return await _httpClient.SendAsync(request);
        }

        public Task<HttpResponseMessage> PutAsync(
            string requestUri)
        {
            var request = new HttpRequestMessageBuilder(
                method: HttpMethod.Put,
                requestUri)
                .Build();

            return _httpClient.SendAsync(request);
        }

        public async Task<HttpResponseMessage> PutWithRoleAsync(
            Role role,
            string requestUri)
        {
            var accessToken = await GetAccessTokenForRoleAsync(role);

            var request = new HttpRequestMessageBuilder(
                method: HttpMethod.Put,
                requestUri)
                .WithAuthorization(
                    scheme: AuthConstants.Scheme,
                    parameter: accessToken)
                .Build();

            return await _httpClient.SendAsync(request);
        }

        public async Task<TValue> GetFromJsonWithRoleAsync<TValue>(
            Role role,
            string requestUri)
        {
            var accessToken = await GetAccessTokenForRoleAsync(role);

            var request = new HttpRequestMessageBuilder(HttpMethod.Get, requestUri)
                .WithAuthorization(
                    scheme: AuthConstants.Scheme,
                    parameter: accessToken)
                .Build();

            return await SendAndParseJsonResponseAsync<TValue>(request);
        }

        private async Task<TValue> SendAndParseJsonResponseAsync<TValue>(
            HttpRequestMessage requestMessage)
        {
            var response = await _httpClient.SendAsync(requestMessage);

            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<TValue>();

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
