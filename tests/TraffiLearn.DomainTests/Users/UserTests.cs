using FluentAssertions;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Testing.Shared.Factories;

namespace TraffiLearn.DomainTests.Users
{
    public sealed class UserTests
    {
#pragma warning disable CS8625

        [Fact]
        public void Create_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            Action[] actions = [
                () =>
                {
                    User.Create(
                    new UserId(Guid.NewGuid()),
                    UserFixtureFactory.CreateEmail(),
                    null,
                    UserFixtureFactory.CreateRole());
                },
                () =>
                {
                    User.Create(
                    new UserId(Guid.NewGuid()),
                    null,
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
            user.LikedQuestions.Should().BeEmpty();
            user.DislikedComments.Should().BeEmpty();
            user.LikedComments.Should().BeEmpty();
            user.DislikedComments.Should().BeEmpty();
            user.MarkedQuestions.Should().BeEmpty();
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
        public void AddComment_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var user = UserFixtureFactory.CreateUser();

            Action action = () =>
            {
                user.AddComment(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddComment_IfCommentAlreadyAdded_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();

            var comment = CommentFixtureFactory.CreateComment();

            user.AddComment(comment);
            var result = user.AddComment(comment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void AddComment_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var user = UserFixtureFactory.CreateUser();

            var countBefore = user.Comments.Count();

            var comment = CommentFixtureFactory.CreateComment();

            var result = user.AddComment(comment);

            result.IsSuccess.Should().BeTrue();
            user.Comments.Should().HaveCount(countBefore + 1);
            user.Comments.Should().Contain(comment);
        }

        [Fact]
        public void MarkQuestion_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var user = UserFixtureFactory.CreateUser();

            Action action = () =>
            {
                user.MarkQuestion(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void MarkQuestion_IfQuestionAlreadyMarked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();

            var question = QuestionFixtureFactory.CreateQuestion();

            user.MarkQuestion(question);
            var result = user.MarkQuestion(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void MarkQuestion_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var user = UserFixtureFactory.CreateUser();

            var countBefore = user.MarkedQuestions.Count();

            var question = QuestionFixtureFactory.CreateQuestion();

            var result = user.MarkQuestion(question);

            result.IsSuccess.Should().BeTrue();
            user.MarkedQuestions.Should().HaveCount(countBefore + 1);
            user.MarkedQuestions.Should().Contain(question);
        }

        [Fact]
        public void UnmarkQuestion_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var user = UserFixtureFactory.CreateUser();

            Action action = () =>
            {
                user.UnmarkQuestion(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void UnmarkQuestion_IfQuestionNotMarked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();

            var question = QuestionFixtureFactory.CreateQuestion();

            var result = user.UnmarkQuestion(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void UnmarkQuestion_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var user = UserFixtureFactory.CreateUser();

            var question = QuestionFixtureFactory.CreateQuestion();

            user.MarkQuestion(question);

            var countBefore = user.MarkedQuestions.Count();

            var result = user.UnmarkQuestion(question);

            result.IsSuccess.Should().BeTrue();
            user.MarkedQuestions.Should().HaveCount(countBefore - 1);
            user.MarkedQuestions.Should().NotContain(question);
        }

        [Fact]
        public void LikeQuestion_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var user = UserFixtureFactory.CreateUser();

            Action action = () =>
            {
                user.LikeQuestion(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LikeQuestion_IfAlreadyLiked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion();

            user.LikeQuestion(question);

            var result = user.LikeQuestion(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void LikeQuestion_IfAlreadyDisliked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion();

            user.DislikeQuestion(question);

            var result = user.LikeQuestion(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void LikeQuestion_IfPassedValidArgs_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion();

            var result = user.LikeQuestion(question);

            result.IsSuccess.Should().BeTrue();

            user.LikedQuestions.Should().HaveCount(1);
            user.LikedQuestions.Should().Contain(question);
        }

        [Fact]
        public void DislikeQuestion_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var user = UserFixtureFactory.CreateUser();

            Action action = () =>
            {
                user.DislikeQuestion(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void DislikeQuestion_IfAlreadyDisliked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion();

            user.DislikeQuestion(question);

            var result = user.DislikeQuestion(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void DislikeQuestion_IfAlreadyLiked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion();

            user.LikeQuestion(question);

            var result = user.DislikeQuestion(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void DislikeQuestion_IfPassedValidArgs_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion();

            var result = user.DislikeQuestion(question);

            result.IsSuccess.Should().BeTrue();

            user.DislikedQuestions.Should().HaveCount(1);
            user.DislikedQuestions.Should().Contain(question);
        }

        [Fact]
        public void RemoveQuestionLike_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var user = UserFixtureFactory.CreateUser();

            Action action = () =>
            {
                user.RemoveQuestionLike(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RemoveQuestionLike_IfNotLiked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion();

            var result = user.RemoveQuestionLike(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveQuestionLike_IfValidCase_ShouldBeSuccesful()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion();

            user.LikeQuestion(question);

            var countBefore = user.LikedQuestions.Count;

            var result = user.RemoveQuestionLike(question);

            result.IsSuccess.Should().BeTrue();

            user.LikedQuestions.Should().HaveCount(countBefore - 1);
            user.LikedQuestions.Should().NotContain(question);
        }

        [Fact]
        public void RemoveQuestionDislike_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var user = UserFixtureFactory.CreateUser();

            Action action = () =>
            {
                user.RemoveQuestionDislike(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RemoveQuestionDislike_IfNotLiked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion();

            var result = user.RemoveQuestionDislike(question);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveQuestionDislike_IfValidCase_ShouldBeSuccesful()
        {
            var user = UserFixtureFactory.CreateUser();
            var question = QuestionFixtureFactory.CreateQuestion();

            user.DislikeQuestion(question);

            var countBefore = user.DislikedQuestions.Count;

            var result = user.RemoveQuestionDislike(question);

            result.IsSuccess.Should().BeTrue();

            user.DislikedQuestions.Should().HaveCount(countBefore - 1);
            user.DislikedQuestions.Should().NotContain(question);
        }

        [Fact]
        public void LikeComment_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var user = UserFixtureFactory.CreateUser();

            Action action = () =>
            {
                user.LikeComment(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void LikeComment_IfAlreadyLiked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment();

            user.LikeComment(comment);

            var result = user.LikeComment(comment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void LikeComment_IfAlreadyDisliked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment();

            user.DislikeComment(comment);

            var result = user.LikeComment(comment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void LikeComment_IfPassedValidArgs_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment();

            var result = user.LikeComment(comment);

            result.IsSuccess.Should().BeTrue();

            user.LikedComments.Should().HaveCount(1);
            user.LikedComments.Should().Contain(comment);
        }

        [Fact]
        public void DislikeComment_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var user = UserFixtureFactory.CreateUser();

            Action action = () =>
            {
                user.DislikeComment(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void DislikeComment_IfAlreadyDisliked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment();

            user.DislikeComment(comment);

            var result = user.DislikeComment(comment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void DislikeComment_IfAlreadyLiked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment();

            user.LikeComment(comment);

            var result = user.DislikeComment(comment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void DislikeComment_IfPassedValidArgs_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment();

            var result = user.DislikeComment(comment);

            result.IsSuccess.Should().BeTrue();

            user.DislikedComments.Should().HaveCount(1);
            user.DislikedComments.Should().Contain(comment);
        }

        [Fact]
        public void RemoveCommentLike_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var user = UserFixtureFactory.CreateUser();

            Action action = () =>
            {
                user.RemoveCommentLike(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RemoveCommentLike_IfNotLiked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment();

            var result = user.RemoveCommentLike(comment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveCommentLike_IfValidCase_ShouldBeSuccesful()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment();

            user.LikeComment(comment);

            var countBefore = user.LikedComments.Count;

            var result = user.RemoveCommentLike(comment);

            result.IsSuccess.Should().BeTrue();

            user.LikedComments.Should().HaveCount(countBefore - 1);
            user.LikedComments.Should().NotContain(comment);
        }

        [Fact]
        public void RemoveCommentDislike_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var user = UserFixtureFactory.CreateUser();

            Action action = () =>
            {
                user.RemoveCommentDislike(null);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RemoveCommentDislike_IfNotLiked_ShouldReturnError()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment();

            var result = user.RemoveCommentDislike(comment);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveCommentDislike_IfValidCase_ShouldBeSuccesful()
        {
            var user = UserFixtureFactory.CreateUser();
            var comment = CommentFixtureFactory.CreateComment();

            user.DislikeComment(comment);

            var countBefore = user.DislikedComments.Count;

            var result = user.RemoveCommentDislike(comment);

            result.IsSuccess.Should().BeTrue();

            user.DislikedComments.Should().HaveCount(countBefore - 1);
            user.DislikedComments.Should().NotContain(comment);
        }

        [Fact]
        public void User_ShouldInheritFromEntity()
        {
            var type = typeof(User);

            var isValueObject = typeof(Entity<UserId>).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("User should inherit from Entity.");
        }

#pragma warning restore
    }
}
