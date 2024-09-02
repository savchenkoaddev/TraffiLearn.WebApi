using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;

namespace TraffiLearn.Testing.Shared.Factories
{
    public static class CommentFixtureFactory
    {
        public static Comment CreateComment(string content = "Content")
        {
            return Comment.Create(
                new CommentId(Guid.NewGuid()),
                CreateContent(content),
                UserFixtureFactory.CreateUser(),
                QuestionFixtureFactory.CreateQuestion()).Value;
        }

        public static CommentContent CreateContent(string content = "Content")
        {
            return CommentContent.Create(content).Value;
        }
    }
}
