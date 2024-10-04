namespace TraffiLearn.Application.ServiceCenters.DTO
{
    public sealed record ServiceCenterResponse(
        Guid Id,
        string LocationName,
        string RoadName,
        string BuildingNumber);
}
