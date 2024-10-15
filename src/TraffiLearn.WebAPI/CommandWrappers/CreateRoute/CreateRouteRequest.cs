namespace TraffiLearn.WebAPI.CommandWrappers.CreateRoute
{
    public sealed record CreateRouteRequest(
        Guid? ServiceCenterId,
        int? RouteNumber,
        string? Description);
}
