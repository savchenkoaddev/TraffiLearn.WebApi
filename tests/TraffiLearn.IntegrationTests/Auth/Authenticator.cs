using System.Net.Http.Json;
using System.Text.Json;
using TraffiLearn.Application.Auth.DTO;

namespace TraffiLearn.IntegrationTests.Auth
{
    internal sealed class Authenticator
    {
        private readonly HttpClient _httpClient;

        public Authenticator(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<LoginResponse> LoginAsOwnerUsingTestCredentialsAsync()
        {
            return LoginAsync(
                email: AuthConstants.OwnerEmail,
                password: AuthConstants.Password);
        }

        public Task<LoginResponse> LoginAsAdminUsingTestCredentialsAsync()
        {
            return LoginAsync(
                email: AuthConstants.AdminEmail,
                password: AuthConstants.Password);
        }

        public Task<LoginResponse> LoginAsRegularUserUsingTestCredentialsAsync()
        {
            return LoginAsync(
                email: AuthConstants.Email,
                password: AuthConstants.Password);
        }

        private async Task<LoginResponse> LoginAsync(
            string email,
            string password)
        {
            var loginResponse = await _httpClient.PostAsJsonAsync(
                requestUri: AuthRoutes.LoginRoute,
                new { email, password });

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
