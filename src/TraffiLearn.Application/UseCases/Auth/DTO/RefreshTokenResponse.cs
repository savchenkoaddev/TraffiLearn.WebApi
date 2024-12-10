namespace TraffiLearn.Application.UseCases.Auth.DTO
{
    public sealed record RefreshTokenResponse(
        string AccessToken,
        string RefreshToken);
}
