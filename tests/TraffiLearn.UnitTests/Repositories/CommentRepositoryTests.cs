using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Comments.CommentContents;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Infrastructure.Persistence.Repositories;
using TraffiLearn.Testing.Shared.Factories;
using TraffiLearn.UnitTests.Abstractions;

namespace TraffiLearn.UnitTests.Repositories
{
    public sealed class CommentRepositoryTests : BaseRepositoryTest
    {
        private readonly CommentRepository _repository;

        public CommentRepositoryTests()
        {
            _repository = new CommentRepository(DbContext);
        }

        [Fact]
        public async Task InsertAsync_ShouldAddComment()
        {
            // Arrange
            var comment = CommentFixtureFactory.CreateComment();

            // Act
            await _repository.InsertAsync(comment);
            await DbContext.SaveChangesAsync();

            // Assert
            DbContext.Comments.Should().Contain(comment);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteComment()
        {
            // Arrange
            var comment = CommentFixtureFactory.CreateComment();

            await AddRangeAndSaveAsync(comment);

            // Act
            await _repository.DeleteAsync(comment);
            await DbContext.SaveChangesAsync();

            // Assert
            DbContext.Comments.Should().NotContain(comment);
        }

        [Fact]
        public async Task ExistsAsync_IfCommentExists_ShouldReturnTrue()
        {
            // Arrange
            var comment = CommentFixtureFactory.CreateComment();

            await AddRangeAndSaveAsync(comment);

            // Act
            var result = await _repository.ExistsAsync(comment.Id);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsAsync_IfCommentDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var commentId = new CommentId(Guid.NewGuid());

            // Act
            var result = await _repository.ExistsAsync(commentId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateComment()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            var question = QuestionFixtureFactory.CreateQuestion();

            var comment = Comment.Create(
                new CommentId(Guid.NewGuid()),
                CommentContent.Create("Content").Value,
                user,
                question)
                .Value;

            var updatedComment = Comment.Create(
                comment.Id,
                CommentContent.Create("Test content").Value,
                comment.Creator,
                comment.Question)
                .Value;

            await AddRangeAndSaveAsync(comment, user, question);
            DbContext.Entry(comment).State = EntityState.Detached;

            // Act
            await _repository.UpdateAsync(updatedComment);
            await DbContext.SaveChangesAsync();

            // Assert
            DbContext.Comments.Should().Contain(updatedComment);
            DbContext.Comments.First(q => q.Id == comment.Id)
                .Should().BeEquivalentTo(updatedComment);
        }

        [Fact]
        public async Task GetByIdAsync_IfCommentDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var commentId = new CommentId(Guid.NewGuid());

            // Act
            var result = await _repository.GetByIdAsync(commentId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetUserCreatedCommentsAsync_IfUserExists_ShouldReturnUserCreatedComments()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            var question1 = QuestionFixtureFactory.CreateQuestion(number: 1);

            var comment1 = Comment.Create(
                new CommentId(Guid.NewGuid()),
                CommentContent.Create("Test content").Value,
                user,
                question1)
                .Value;

            var question2 = QuestionFixtureFactory.CreateQuestion(number: 2);

            var comment2 = Comment.Create(
                new CommentId(Guid.NewGuid()),
                CommentContent.Create("Test content").Value,
                user,
                question2)
                .Value;

            await AddRangeAndSaveAsync(user, question1, comment1, question2, comment2);

            // Act
            var result = await _repository.GetUserCreatedCommentsAsync(user.Id);

            // Assert
            result.Should()
                .Contain(comment1)
                .And
                .Contain(comment2);
        }

        [Fact]
        public async Task GetUserCreatedCommentsAsync_IfUserDoNotExist_ShouldReturnEmptyCollection()
        {
            // Arrange
            var userId = new UserId(Guid.NewGuid());

            // Act
            var result = await _repository.GetUserCreatedCommentsAsync(userId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetUserCreatedCommentsAsync_IfUserHasNoComments_ShouldReturnEmptyCollection()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            await AddRangeAndSaveAsync(user);

            // Act
            var result = await _repository.GetUserCreatedCommentsAsync(user.Id);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetRepliesWithNextRepliesByIdAsync_IfCommentExists_ShouldReturnRepliesWithNextReplies()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            var question = QuestionFixtureFactory.CreateQuestion();

            var comment = Comment.Create(
                new CommentId(Guid.NewGuid()),
                CommentContent.Create("comment").Value,
                user,
                question)
                .Value;

            var reply1 = Comment.Create(
                new CommentId(Guid.NewGuid()),
                CommentContent.Create("reply1").Value,
                user,
                question)
                .Value;

            var reply2 = Comment.Create(
                new CommentId(Guid.NewGuid()),
                CommentContent.Create("reply2").Value,
                user,
                question)
                .Value;

            var reply3 = Comment.Create(
                new CommentId(Guid.NewGuid()),
                CommentContent.Create("reply3").Value,
                user,
                question)
                .Value;

            var reply4 = Comment.Create(
                new CommentId(Guid.NewGuid()),
                CommentContent.Create("reply4").Value,
                user,
                question)
                .Value;

            comment.Reply(reply1);
            comment.Reply(reply2);
            reply1.Reply(reply3);
            reply1.Reply(reply4);

            await AddRangeAndSaveAsync(comment, reply1, reply2, reply3, reply4);

            // Act
            var result = await _repository.GetRepliesWithNextRepliesByIdAsync(comment.Id);

            // Assert
            result.Should()
                .Contain(comment).Which.Replies.Should()
                .Contain(reply1)
                .And
                .Contain(reply2);
            result.First(c => c.Id == comment.Id)
                .Replies.First(r => r.Id == reply1.Id)
                .Replies.Should()
                .Contain(reply3)
                .And
                .Contain(reply4);
        }

        [Fact]
        public async Task GetRepliesWithNextRepliesByIdAsync_IfCommentDoesNotExist_ShouldReturnEmptyCollection()
        {
            // Arrange
            var commentId = new CommentId(Guid.NewGuid());

            // Act
            var result = await _repository.GetRepliesWithNextRepliesByIdAsync(commentId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetRepliesWithNextRepliesByIdAsync_IfCommentHasNoReplies_ShouldReturnCommentWithNoReplies()
        {
            // Arrange
            var comment = CommentFixtureFactory.CreateComment();

            await AddRangeAndSaveAsync(comment);

            // Act
            var result = await _repository.GetRepliesWithNextRepliesByIdAsync(comment.Id);

            // Assert
            result.First(c => c.Id == comment.Id)
                .Replies.Should().BeEmpty();

        }

        [Fact]
        public async Task GetManyByQuestionIdWithRepliesAndCreatorsAsync_IfQuestionExists_ShouldReturnCommentsWithRepliesAndCreators()
        {
            // Arrange
            var user1 = UserFixtureFactory.CreateUser(email: "test1@gmail.com", username: "test1");

            var user2 = UserFixtureFactory.CreateUser(email: "test2@gmail.com", username: "test2");

            var question = QuestionFixtureFactory.CreateQuestion();

            var comment1 = Comment.Create(
                new CommentId(Guid.NewGuid()),
                CommentContent.Create("comment1").Value,
                user1,
                question)
                .Value;

            var comment2 = Comment.Create(
                new CommentId(Guid.NewGuid()),
                CommentContent.Create("comment2").Value,
                user2,
                question)
                .Value;

            var reply1 = Comment.Create(
                new CommentId(Guid.NewGuid()),
                CommentContent.Create("reply1").Value,
                user1,
                question)
                .Value;

            var reply2 = Comment.Create(
                new CommentId(Guid.NewGuid()),
                CommentContent.Create("reply2").Value,
                user2,
                question)
                .Value;

            comment1.Reply(reply1);
            comment2.Reply(reply2);

            await AddRangeAndSaveAsync(user1, user2, question,
                comment1, comment2, reply1, reply2);

            // Act
            var result = await _repository.GetManyByQuestionIdWithRepliesAndCreatorsAsync(
                question.Id,
                page: 1,
                pageSize: 10);

            // Assert
            result.First(c => c.Id == comment1.Id)
                .Replies.Should().Contain(reply1);
            result.First(c => c.Id == comment2.Id)
                .Replies.Should().Contain(reply2);

            result.Should()
                .Contain(comment1)
                .Which.Creator.Should().Be(user1);
            result.Should()
                .Contain(comment2)
                .Which.Creator.Should().Be(user2);
            result.First(c => c.Id == comment1.Id)
                .Replies.Should()
                .Contain(reply1)
                .Which.Creator.Should().Be(user1);
            result.First(c => c.Id == comment2.Id)
                .Replies.Should()
                .Contain(reply2)
                .Which.Creator.Should().Be(user2);
        }
    }
}
