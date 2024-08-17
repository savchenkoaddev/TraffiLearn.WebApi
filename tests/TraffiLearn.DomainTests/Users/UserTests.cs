using FluentAssertions;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.DomainTests.Factories;

namespace TraffiLearn.DomainTests.Users
{
    public sealed class UserTests
    {
        [Fact]
        public void Create_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            Action[] actions = [
                () =>
                {
                    User.Create(
                    new UserId(Guid.NewGuid()),
                    UserFixtureFactory.CreateEmail(),
                    null!,
                    UserFixtureFactory.CreateRole());
                },
                () =>
                {
                    User.Create(
                    new UserId(Guid.NewGuid()),
                    null!,
                    UserFixtureFactory.CreateUsername(),
                    UserFixtureFactory.CreateRole());
                }
            ];

            actions.Should().AllSatisfy(action =>
                action.Should().Throw<ArgumentNullException>());
        }

        [Fact]
        public void Create_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var id = new UserId(Guid.NewGuid());
            var email = UserFixtureFactory.CreateEmail();
            var username = UserFixtureFactory.CreateUsername();
            var role = UserFixtureFactory.CreateRole();

            var result = User.Create(
                id,
                email,
                username,
                role);

            result.IsSuccess.Should().BeTrue();

            var user = result.Value;

            user.Id.Should().Be(id);
            user.Email.Should().Be(email);
            user.Username.Should().Be(username);
            user.Role.Should().Be(role);
            user.LikedQuestionsIds.Should().BeEmpty();
            user.DislikedCommentsIds.Should().BeEmpty();
            user.LikedCommentsIds.Should().BeEmpty();
            user.DislikedCommentsIds.Should().BeEmpty();
            user.MarkedQuestionsIds.Should().BeEmpty();
        }

        [Fact]
        public void DowngradeRole_IfRoleIsAlreadyLowest_ShouldReturnError()
        {
            var lowestRole = Enum.GetValues(typeof(Role)).Cast<Role>().OrderBy(r => r).First();
            var email = UserFixtureFactory.CreateEmail();
            var username = UserFixtureFactory.CreateUsername();

            var user = User.Create(
                new UserId(Guid.NewGuid()),
                email,
                username,
                lowestRole).Value;

            var result = user.DowngradeRole();

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void DowngradeRole_IfRoleIsNotLowest_ShouldBeSuccesful()
        {
            var roles = Enum.GetValues(typeof(Role))
                .Cast<Role>()
                .OrderBy(r => r);

            var role = roles.Skip(1).First();
            var expectedRole = roles.First();

            var email = UserFixtureFactory.CreateEmail();
            var username = UserFixtureFactory.CreateUsername();

            var user = User.Create(
                new UserId(Guid.NewGuid()),
                email,
                username,
                role).Value;

            var result = user.DowngradeRole();

            result.IsSuccess.Should().BeTrue();
            user.Role.Should().Be(expectedRole);
        }

        [Fact]
        public void AddComment_IfCommentAlreadyAdded_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();

            var comment = CommentFixtureFactory.CreateComment().Id;

            user.AddComment(comment);
            var result = user.AddComment(comment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void AddComment_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var user = UserFixtureFactory.CreateUser();

            var countBefore = user.CommentsIds.Count();

            var comment = CommentFixtureFactory.CreateComment().Id;

            var result = user.AddComment(comment);

            result.IsSuccess.Should().BeTrue();
            user.CommentsIds.Should().HaveCount(countBefore + 1);
            user.CommentsIds.Should().Contain(comment);
        }

        [Fact]
        public void MarkQuestion_IfQuestionAlreadyMarked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();

            var question = QuestionFixtureFactory.CreateQuestion().Id;

            user.MarkQuestion(question);
            var result = user.MarkQuestion(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void MarkQuestion_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var user = UserFixtureFactory.CreateUser();

            var countBefore = user.MarkedQuestionsIds.Count();

            var question = QuestionFixtureFactory.CreateQuestion().Id;

            var result = user.MarkQuestion(question);

            result.IsSuccess.Should().BeTrue();
            user.MarkedQuestionsIds.Should().HaveCount(countBefore + 1);
            user.MarkedQuestionsIds.Should().Contain(question);
        }

        [Fact]
        public void UnmarkQuestion_IfQuestionNotMarked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();

            var question = QuestionFixtureFactory.CreateQuestion().Id;

            var result = user.UnmarkQuestion(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void UnmarkQuestion_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var user = UserFixtureFactory.CreateUser();

            var question = QuestionFixtureFactory.CreateQuestion().Id;

            user.MarkQuestion(question);

            var countBefore = user.MarkedQuestionsIds.Count();

            var result = user.UnmarkQuestion(question);

            result.IsSuccess.Should().BeTrue();
            user.MarkedQuestionsIds.Should().HaveCount(countBefore - 1);
            user.MarkedQuestionsIds.Should().NotContain(question);
        }

        [Fact]
        public void LikeQuestion_IfAlreadyLiked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion().Id;

            user.LikeQuestion(question);

            var result = user.LikeQuestion(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void LikeQuestion_IfAlreadyDisliked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion().Id;

            user.DislikeQuestion(question);

            var result = user.LikeQuestion(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void LikeQuestion_IfPassedValidArgs_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion().Id;

            var result = user.LikeQuestion(question);

            result.IsSuccess.Should().BeTrue();

            user.LikedQuestionsIds.Should().HaveCount(1);
            user.LikedQuestionsIds.Should().Contain(question);
        }

        [Fact]
        public void DislikeQuestion_IfAlreadyDisliked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion().Id;

            user.DislikeQuestion(question);

            var result = user.DislikeQuestion(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void DislikeQuestion_IfAlreadyLiked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion().Id;

            user.LikeQuestion(question);

            var result = user.DislikeQuestion(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void DislikeQuestion_IfPassedValidArgs_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion().Id;

            var result = user.DislikeQuestion(question);

            result.IsSuccess.Should().BeTrue();

            user.DislikedQuestionsIds.Should().HaveCount(1);
            user.DislikedQuestionsIds.Should().Contain(question);
        }

        [Fact]
        public void RemoveQuestionLike_IfNotLiked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion().Id;

            var result = user.RemoveQuestionLike(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveQuestionLike_IfValidCase_ShouldBeSuccesful()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion().Id;

            user.LikeQuestion(question);

            var countBefore = user.LikedQuestionsIds.Count;

            var result = user.RemoveQuestionLike(question);

            result.IsSuccess.Should().BeTrue();

            user.LikedQuestionsIds.Should().HaveCount(countBefore - 1);
            user.LikedQuestionsIds.Should().NotContain(question);
        }

        [Fact]
        public void RemoveQuestionDislike_IfNotLiked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion().Id;

            var result = user.RemoveQuestionDislike(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveQuestionDislike_IfValidCase_ShouldBeSuccesful()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion().Id;

            user.DislikeQuestion(question);

            var countBefore = user.DislikedQuestionsIds.Count;

            var result = user.RemoveQuestionDislike(question);

            result.IsSuccess.Should().BeTrue();

            user.DislikedQuestionsIds.Should().HaveCount(countBefore - 1);
            user.DislikedQuestionsIds.Should().NotContain(question);
        }

        [Fact]
        public void LikeComment_IfAlreadyLiked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment().Id;

            user.LikeComment(comment);

            var result = user.LikeComment(comment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void LikeComment_IfAlreadyDisliked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment().Id;

            user.DislikeComment(comment);

            var result = user.LikeComment(comment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void LikeComment_IfPassedValidArgs_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment().Id;

            var result = user.LikeComment(comment);

            result.IsSuccess.Should().BeTrue();

            user.LikedCommentsIds.Should().HaveCount(1);
            user.LikedCommentsIds.Should().Contain(comment);
        }

        [Fact]
        public void DislikeComment_IfAlreadyDisliked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment().Id;

            user.DislikeComment(comment);

            var result = user.DislikeComment(comment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void DislikeComment_IfAlreadyLiked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment().Id;

            user.LikeComment(comment);

            var result = user.DislikeComment(comment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void DislikeComment_IfPassedValidArgs_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment().Id;

            var result = user.DislikeComment(comment);

            result.IsSuccess.Should().BeTrue();

            user.DislikedCommentsIds.Should().HaveCount(1);
            user.DislikedCommentsIds.Should().Contain(comment);
        }

        [Fact]
        public void RemoveCommentLike_IfNotLiked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment().Id;

            var result = user.RemoveCommentLike(comment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveCommentLike_IfValidCase_ShouldBeSuccesful()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment().Id;

            user.LikeComment(comment);

            var countBefore = user.LikedCommentsIds.Count;

            var result = user.RemoveCommentLike(comment);

            result.IsSuccess.Should().BeTrue();

            user.LikedCommentsIds.Should().HaveCount(countBefore - 1);
            user.LikedCommentsIds.Should().NotContain(comment);
        }

        [Fact]
        public void RemoveCommentDislike_IfNotLiked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment().Id;

            var result = user.RemoveCommentDislike(comment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveCommentDislike_IfValidCase_ShouldBeSuccesful()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment().Id;

            user.DislikeComment(comment);

            var countBefore = user.DislikedCommentsIds.Count;

            var result = user.RemoveCommentDislike(comment);

            result.IsSuccess.Should().BeTrue();

            user.DislikedCommentsIds.Should().HaveCount(countBefore - 1);
            user.DislikedCommentsIds.Should().NotContain(comment);
        }

        [Fact]
        public void User_ShouldInheritFromEntity()
        {
            var type = typeof(User);

            var isValueObject = typeof(Entity<UserId>).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("User should inherit from Entity.");
        }
    }
}
