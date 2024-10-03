namespace TraffiLearn.Application.Auth.DTO
{
    public sealed record RefreshTokenResponse(
        string AccessToken,
        string RefreshToken);
}
