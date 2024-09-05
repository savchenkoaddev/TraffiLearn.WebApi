using System.Net.Http.Json;
using System.Text.Json;
using TraffiLearn.Application.Auth.DTO;

namespace TraffiLearn.IntegrationTests.Auth
{
    public sealed class Authenticator
    {
        private readonly HttpClient _httpClient;

        public Authenticator(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse> LoginAsync(
            RoleCredentials credentials)
        {
            var loginResponse = await _httpClient.PostAsJsonAsync(
                requestUri: AuthRoutes.LoginRoute,
                new
                {
                    email = credentials.Email,
                    password = credentials.Password
                });

            loginResponse.EnsureSuccessStatusCode();

            var jsonResponse = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();

            if (jsonResponse is null)
            {
                throw new JsonException("Failed to deserialize login response.");
            }

            return jsonResponse;
        }
    }
}
