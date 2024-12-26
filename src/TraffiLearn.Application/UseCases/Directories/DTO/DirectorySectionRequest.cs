namespace TraffiLearn.Application.UseCases.Directories.DTO
{
    public sealed record DirectorySectionRequest(
        string Name,
        string Content);
}