namespace TraffiLearn.Application.Routes.DTO
{
    public sealed record RouteResponse(
        Guid Id,
        int RouteNumber,
        string? Description,
        string ImageUri);
}
