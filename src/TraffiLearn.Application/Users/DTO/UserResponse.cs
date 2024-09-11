namespace TraffiLearn.Application.Users.DTO
{
    public sealed record UserResponse(
        Guid Id,
        string Email,
        string Username);
}
