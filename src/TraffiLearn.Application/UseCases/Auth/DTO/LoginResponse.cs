namespace TraffiLearn.Application.UseCases.Auth.DTO
{
    public sealed record LoginResponse(
        string AccessToken,
        string RefreshToken);
}
