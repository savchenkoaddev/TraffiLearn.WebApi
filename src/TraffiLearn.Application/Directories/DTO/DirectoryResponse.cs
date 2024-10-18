namespace TraffiLearn.Application.Directories.DTO
{
    public sealed record DirectoryResponse(
        Guid Id,
        string Name,
        IEnumerable<DirectorySectionResponse> Sections);
}
