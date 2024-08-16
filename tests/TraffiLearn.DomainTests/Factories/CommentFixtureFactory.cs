using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;

namespace TraffiLearn.DomainTests.Factories
{
    internal static class CommentFixtureFactory
    {
        public static Comment CreateComment()
        {
            return Comment.Create(
                new CommentId(Guid.NewGuid()),
                CreateContent(),
                UserFixtureFactory.CreateUser(),
                QuestionFixtureFactory.CreateQuestion()).Value;
        }

        public static CommentContent CreateContent()
        {
            return CommentContent.Create("Content").Value;
        }
    }
}
