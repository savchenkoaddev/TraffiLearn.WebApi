using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Comments.DTO;
using TraffiLearn.Domain.Comments;

namespace TraffiLearn.Application.UseCases.Comments.Mappers
{
    internal sealed class CommentToCommentResponseMapper
        : Mapper<Comment, CommentResponse>
    {
        public override CommentResponse Map(Comment source)
        {
            bool hasReplies = source.Replies.Count > 0;

            return new CommentResponse(
                CommentId: source.Id.Value,
                AuthorUserId: source.Creator.Id.Value,
                AuthorUsername: source.Creator.Username.Value,
                Content: source.Content.Value,
                HasReplies: hasReplies);
        }
    }
}
