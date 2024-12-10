namespace TraffiLearn.Application.UseCases.Comments.DTO
{
    public sealed record PaginatedCommentsResponse(
        IEnumerable<CommentResponse> Comments,
        int TotalPages);
}
