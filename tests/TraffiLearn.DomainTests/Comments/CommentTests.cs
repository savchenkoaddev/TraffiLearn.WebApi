﻿using FluentAssertions;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.DomainTests.Factories;

namespace TraffiLearn.DomainTests.Comments
{
    public sealed class CommentTests
    {
        [Fact]
        public void Create_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            Action action = () =>
            {
                Comment.Create(
                        new CommentId(),
                        null!,
                        UserFixtureFactory.CreateUser().Id,
                        QuestionFixtureFactory.CreateQuestion().Id);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Create_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var id = new CommentId(Guid.NewGuid());
            var content = CommentFixtureFactory.CreateContent();
            var creator = UserFixtureFactory.CreateUser().Id;
            var question = QuestionFixtureFactory.CreateQuestion().Id;

            var result = Comment.Create(
                id,
                content,
                creator,
                question);

            result.IsSuccess.Should().BeTrue();

            var comment = result.Value;

            comment.Content.Should().Be(content);
            comment.CreatorId.Should().Be(creator);
            comment.QuestionId.Should().Be(question);
            comment.Id.Should().Be(id);

            comment.DislikesCount.Should().Be(0);
            comment.LikesCount.Should().Be(0);
            comment.LikedByUsersIds.Should().BeEmpty();
            comment.DislikedByUsersIds.Should().BeEmpty();
            comment.Replies.Should().BeEmpty();
        }

        [Fact]
        public void AddLike_IfUserAlreadyLiked_ShouldReturnError()
        {
            var comment = CommentFixtureFactory.CreateComment();

            var user = UserFixtureFactory.CreateUser().Id;

            comment.AddLike(user);
            var result = comment.AddLike(user);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void AddLike_IfUserAlreadyDisliked_ShouldReturnError()
        {
            var comment = CommentFixtureFactory.CreateComment();

            var user = UserFixtureFactory.CreateUser().Id;

            comment.AddDislike(user);
            var result = comment.AddLike(user);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void AddLike_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var comment = CommentFixtureFactory.CreateComment();

            var countBefore = comment.LikedByUsersIds.Count();
            var likesBefore = comment.LikesCount;

            var user = UserFixtureFactory.CreateUser().Id;

            var result = comment.AddLike(user);

            result.IsSuccess.Should().BeTrue();
            comment.LikedByUsersIds.Should().HaveCount(countBefore + 1);
            comment.LikedByUsersIds.Should().Contain(user);
            comment.LikesCount.Should().Be(likesBefore + 1);
        }

        [Fact]
        public void AddDislike_IfUserAlreadyDisliked_ShouldReturnError()
        {
            var comment = CommentFixtureFactory.CreateComment();

            var user = UserFixtureFactory.CreateUser().Id;

            comment.AddDislike(user);
            var result = comment.AddDislike(user);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void AddDislike_IfUserAlreadyLiked_ShouldReturnError()
        {
            var comment = CommentFixtureFactory.CreateComment();

            var user = UserFixtureFactory.CreateUser().Id;

            comment.AddLike(user);
            var result = comment.AddDislike(user);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void AddDislike_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var comment = CommentFixtureFactory.CreateComment();

            var countBefore = comment.DislikedByUsersIds.Count();
            var dislikesBefore = comment.DislikesCount;

            var user = UserFixtureFactory.CreateUser().Id;

            var result = comment.AddDislike(user);

            result.IsSuccess.Should().BeTrue();
            comment.DislikedByUsersIds.Should().HaveCount(countBefore + 1);
            comment.DislikedByUsersIds.Should().Contain(user);
            comment.DislikesCount.Should().Be(dislikesBefore + 1);
        }

        [Fact]
        public void RemoveLike_IfCommentNotLiked_ShouldReturnError()
        {
            var comment = CommentFixtureFactory.CreateComment();

            var user = UserFixtureFactory.CreateUser().Id;

            var result = comment.RemoveLike(user);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveLike_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var comment = CommentFixtureFactory.CreateComment();

            var user = UserFixtureFactory.CreateUser().Id;

            comment.AddLike(user);

            var countBefore = comment.LikedByUsersIds.Count();
            var likesBefore = comment.LikesCount;

            var result = comment.RemoveLike(user);

            result.IsSuccess.Should().BeTrue();

            comment.LikedByUsersIds.Should().HaveCount(countBefore - 1);
            comment.LikedByUsersIds.Should().NotContain(user);

            comment.LikesCount.Should().Be(likesBefore - 1);
        }

        [Fact]
        public void RemoveDislike_IfCommentNotDisliked_ShouldReturnError()
        {
            var comment = CommentFixtureFactory.CreateComment();

            var user = UserFixtureFactory.CreateUser().Id;

            var result = comment.RemoveDislike(user);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void RemoveDislike_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var comment = CommentFixtureFactory.CreateComment();

            var user = UserFixtureFactory.CreateUser().Id;

            comment.AddDislike(user);

            var countBefore = comment.DislikedByUsersIds.Count();
            var dislikesBefore = comment.DislikesCount;

            var result = comment.RemoveDislike(user);

            result.IsSuccess.Should().BeTrue();

            comment.DislikedByUsersIds.Should().HaveCount(countBefore - 1);
            comment.DislikedByUsersIds.Should().NotContain(user);

            comment.DislikesCount.Should().Be(dislikesBefore - 1);
        }

        [Fact]
        public void Update_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var comment = CommentFixtureFactory.CreateComment();

            Action action = () =>
            {
                comment.Update(null!);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var comment = CommentFixtureFactory.CreateComment();

            var newContent = CommentContent.Create(Guid.NewGuid().ToString()).Value;

            var updateResult = comment.Update(newContent);

            updateResult.IsSuccess.Should().BeTrue();
            comment.Content.Should().Be(newContent);
        }

        [Fact]
        public void Reply_IfPassedNullArgs_ShouldThrowArgumentNullException()
        {
            var comment = CommentFixtureFactory.CreateComment();

            Action action = () =>
            {
                comment.Reply(null!);
            };

            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Reply_IfPassedInvalidArgs_ShouldReturnError()
        {
            var comment = CommentFixtureFactory.CreateComment();

            var reply = CommentFixtureFactory.CreateComment();

            comment.Reply(reply);

            var result = comment.Reply(reply);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void Reply_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var comment = CommentFixtureFactory.CreateComment();
            var reply = CommentFixtureFactory.CreateComment();

            var result = comment.Reply(reply);

            result.IsSuccess.Should().BeTrue();

            comment.Replies.Should().HaveCount(1);
            comment.Replies.Should().Contain(reply);

            reply.RootComment.Should().NotBeNull();
            reply.RootComment.Should().Be(comment);
        }

        [Fact]
        public void Comment_ShouldInheritFromEntity()
        {
            var type = typeof(Comment);

            var isValueObject = typeof(Entity<CommentId>).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("Comment should inherit from Entity.");
        }
    }
}
