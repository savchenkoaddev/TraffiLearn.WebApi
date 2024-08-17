namespace TraffiLearn.Application.DTO.Comments
{
    public sealed record CommentResponse(
        Guid CommentId,
        Guid AuthorUserId,
        string Content,
        bool HasReplies = false);
}