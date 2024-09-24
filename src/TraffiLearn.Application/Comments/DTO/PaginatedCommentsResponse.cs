namespace TraffiLearn.Application.Comments.DTO
{
    public sealed record PaginatedCommentsResponse(
        IEnumerable<CommentResponse> Comments,
        int TotalPages);
}
