namespace TraffiLearn.Application.ServiceCenters.DTO
{
    public sealed record ServiceCenterResponse(
        Guid Id,
        string ServiceCenterNumber,
        string LocationName,
        string RoadName,
        string BuildingNumber);
}
