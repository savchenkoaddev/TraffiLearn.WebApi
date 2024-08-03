namespace TraffiLearn.Application.DTO.Questions
{
    public sealed record QuestionCommentResponse(
        Guid CommentId,
        Guid AuthorUserId,
        string AuthorUsername,
        bool HasReplies = false);
}
