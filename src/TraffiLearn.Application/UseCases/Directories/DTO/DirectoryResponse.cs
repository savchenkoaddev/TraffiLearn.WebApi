namespace TraffiLearn.Application.UseCases.Directories.DTO
{
    public sealed record DirectoryResponse(
        Guid Id,
        string Name,
        IEnumerable<DirectorySectionResponse> Sections);
}
