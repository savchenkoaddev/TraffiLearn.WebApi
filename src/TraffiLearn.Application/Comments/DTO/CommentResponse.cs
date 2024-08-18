namespace TraffiLearn.Application.Comments.DTO
{
    public sealed record CommentResponse(
        Guid CommentId,
        Guid AuthorUserId,
        string AuthorUsername,
        string Content,
        bool HasReplies = false);
}