namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IGoogleAuthService
    {
        Task<bool> IsValidIdToken(string token);
    }
}
