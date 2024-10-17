namespace TraffiLearn.Application.Directories.DTO
{
    public sealed record DirectoryResponse(
        Guid Id,
        string Name,
        List<DirectorySectionResponse> Sections);
}
