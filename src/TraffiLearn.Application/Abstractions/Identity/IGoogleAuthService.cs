namespace TraffiLearn.Application.Abstractions.Identity
{
    public interface IGoogleAuthService
    {
        Task<string> ValidateIdTokenAndGetEmailAsync(string token);
    }
}
