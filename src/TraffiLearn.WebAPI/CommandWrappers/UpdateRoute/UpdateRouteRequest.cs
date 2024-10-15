namespace TraffiLearn.WebAPI.CommandWrappers.UpdateRoute
{
    public sealed record UpdateRouteRequest(
        Guid? RouteId,
        Guid? ServiceCenterId,
        int? RouteNumber,
        string? Description);
}
