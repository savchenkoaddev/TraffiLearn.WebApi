using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.IntegrationTests.Auth;
using TraffiLearn.IntegrationTests.Builders;

namespace TraffiLearn.IntegrationTests.Helpers
{
    public sealed class RequestSender
    {
        private readonly HttpClient _httpClient;
        private readonly Authenticator _authenticator;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly Dictionary<Role, RoleCredentials> _roleCredentials;

        public RequestSender(
            HttpClient httpClient,
            Authenticator authenticator)
        {
            _httpClient = httpClient;
            _authenticator = authenticator;

            _roleCredentials = CreateRoleCredentialsDictionary();
        }

        public async Task<HttpResponseMessage> SendJsonRequestWithRole<TValue>(
            Role role,
            HttpMethod method,
            string requestUri,
            TValue value)
        {
            var credentials = GetCredentialsFromRole(role);

            var accessToken = await GetAccessTokenAsync(credentials);

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

        public async Task<TValue> GetFromJsonWithRoleAsync<TValue>(
            Role role,
            string requestUri)
        {
            var credentials = GetCredentialsFromRole(role);

            var accessToken = await GetAccessTokenAsync(credentials);

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

        private async Task<string> GetAccessTokenAsync(RoleCredentials credentials)
        {
            var loginResponse = await _authenticator.LoginAsync(
                credentials);

            return loginResponse.AccessToken;
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
