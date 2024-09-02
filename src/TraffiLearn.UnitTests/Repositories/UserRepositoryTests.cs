using FluentAssertions;
using TraffiLearn.Domain.Aggregates.Comments;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users;
using TraffiLearn.Domain.Aggregates.Users.Enums;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Infrastructure.Persistence.Repositories;
using TraffiLearn.Testing.Shared.Factories;
using TraffiLearn.UnitTests.Abstractions;

namespace TraffiLearn.UnitTests.Repositories
{
    public sealed class UserRepositoryTests : BaseRepositoryTest
    {
        private readonly UserRepository _repository;

        public UserRepositoryTests()
        {
            _repository = new UserRepository(DbContext);
        }

        [Fact]
        public async Task AddAsync_ShouldAddUser()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            // Act
            await _repository.AddAsync(user);
            await DbContext.SaveChangesAsync();

            // Assert
            DbContext.Users.Should().Contain(user);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteUser()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            await AddRangeAndSaveAsync(user);

            // Act
            var insertedUser = await _repository.GetByIdAsync(user.Id);
            await _repository.DeleteAsync(insertedUser);
            await DbContext.SaveChangesAsync();

            // Assert
            DbContext.Users.Should().NotContain(user);
        }

        [Fact]
        public async Task ExistsAsync_WithUserId_IfUserExists_ShouldReturnTrue()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            await AddRangeAndSaveAsync(user);

            // Act
            var result = await _repository.ExistsAsync(user.Id);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsAsync_WithUserId_IfUserDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var userId = new UserId(Guid.NewGuid());

            // Act
            var result = await _repository.ExistsAsync(userId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ExistsAsync_WithUsername_IfUserExists_ShouldReturnTrue()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            await AddRangeAndSaveAsync(user);

            // Act
            var result = await _repository.ExistsAsync(user.Username);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsAsync_WithUsername_IfUserDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var username = Username.Create("example").Value;

            // Act
            var result = await _repository.ExistsAsync(username);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ExistsAsync_WithUsernameAndEmail_IfUserExists_ShouldReturnTrue()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            await AddRangeAndSaveAsync(user);

            // Act
            var result = await _repository.ExistsAsync(user.Username, user.Email);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsAsync_WithUsernameAndEmail_IfUserDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var username = UserFixtureFactory.CreateUsername();
            var email = UserFixtureFactory.CreateEmail();

            // Act
            var result = await _repository.ExistsAsync(username, email);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetByEmailAsync_IfUserExists_ShouldReturnUser()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            await AddRangeAndSaveAsync(user);

            // Act
            var result = await _repository.GetByEmailAsync(user.Email);

            // Assert
            result.Should().NotBeNull().And.Be(user);
        }

        [Fact]
        public async Task GetByEmailAsync_IfUserDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var email = UserFixtureFactory.CreateEmail();

            // Act
            var result = await _repository.GetByEmailAsync(email);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdAsync_IfUserExists_ShouldReturnUser()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            await AddRangeAndSaveAsync(user);

            // Act
            var result = await _repository.GetByIdAsync(user.Id);

            // Assert
            result.Should().NotBeNull().And.Be(user);
        }

        [Fact]
        public async Task GetByIdAsync_IfUserDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var userId = new UserId(Guid.NewGuid());

            // Act
            var result = await _repository.GetByIdAsync(userId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdWithLikedAndDislikedCommentsAsync_IfUserExists_ShouldReturnUserWithLikedAndDislikedComments()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            #region First comment setup
            var creator1 = UserFixtureFactory.CreateUser(email: "creator1@gmail.com", username: "creator1");

            var question1 = QuestionFixtureFactory.CreateQuestion(number: 1);

            var comment1 = Comment.Create(
                new CommentId(Guid.NewGuid()),
                CommentContent.Create("example comment content").Value,
                creator1,
                question1)
                .Value;
            #endregion

            #region Second comment setup
            var creator2 = UserFixtureFactory.CreateUser(email: "creator2@gmail.com", username: "creator2");

            var question2 = QuestionFixtureFactory.CreateQuestion(number: 2);

            var comment2 = Comment.Create(
                new CommentId(Guid.NewGuid()),
                CommentContent.Create("example comment content").Value,
                creator2,
                question2)
                .Value;
            #endregion

            user.LikeComment(comment1);
            user.DislikeComment(comment2);

            await AddRangeAndSaveAsync(user, creator1, creator2,
                question1, question2, comment1, comment2);

            // Act
            var result = await _repository.GetByIdWithLikedAndDislikedCommentsAsync(user.Id);

            // Assert
            result.Should().NotBeNull().And.Be(user);
            result!.LikedComments.Should().Contain(comment1);
            result.DislikedComments.Should().Contain(comment2);
        }

        [Fact]
        public async Task GetByIdWithLikedAndDislikedCommentsAsync_IfUserDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var userId = new UserId(Guid.NewGuid());

            // Act
            var result = await _repository.GetByIdWithLikedAndDislikedCommentsAsync(userId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdWithLikedAndDislikedQuestionsAsync_IfUserExists_ShouldReturnUserWithLikedAndDislikedQuestions()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            var question1 = QuestionFixtureFactory.CreateQuestion(number: 1);

            var question2 = QuestionFixtureFactory.CreateQuestion(number: 2);

            user.LikeQuestion(question1);
            user.DislikeQuestion(question2);

            await AddRangeAndSaveAsync(user, question1, question2);

            // Act
            var result = await _repository.GetByIdWithLikedAndDislikedQuestionsAsync(user.Id);

            // Assert
            result.Should().NotBeNull().And.Be(user);
            result!.LikedQuestions.Should().Contain(question1);
            result.DislikedQuestions.Should().Contain(question2);
        }

        [Fact]
        public async Task GetByIdWithLikedAndDislikedQuestionsAsync_IfUserDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var userId = new UserId(Guid.NewGuid());

            // Act
            var result = await _repository.GetByIdWithLikedAndDislikedQuestionsAsync(userId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdWithMarkedQuestionsAsync_IfUserExists_ShouldReturnUserWithMarkedQuestions()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            var question1 = QuestionFixtureFactory.CreateQuestion(number: 1);

            var question2 = QuestionFixtureFactory.CreateQuestion(number: 2);

            user.MarkQuestion(question1);
            user.MarkQuestion(question2);

            await AddRangeAndSaveAsync(user, question1, question2);

            // Act
            var result = await _repository.GetByIdWithMarkedQuestionsAsync(user.Id);

            // Assert
            result.Should().NotBeNull().And.Be(user);
            result!.MarkedQuestions.Should()
                .Contain(question1)
                .And
                .Contain(question2);
        }

        [Fact]
        public async Task GetByIdWithMarkedQuestionsAsync_IfUserDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var userId = new UserId(Guid.NewGuid());

            // Act
            var result = await _repository.GetByIdWithMarkedQuestionsAsync(userId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByUsernameAsync_IfUserExists_ShouldReturnUser()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            await AddRangeAndSaveAsync(user);

            // Act
            var result = await _repository.GetByUsernameAsync(user.Username);

            // Assert
            result.Should().NotBeNull().And.Be(user);
        }

        [Fact]
        public async Task GetByUsernameAsync_IfUserDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var username = UserFixtureFactory.CreateUsername();

            // Act
            var result = await _repository.GetByUsernameAsync(username);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateUser()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            var updatedUser = User.Create(
               user.Id,
               Email.Create("example@gmail.com").Value,
               Username.Create("new_username").Value,
               Role.Admin)
               .Value;

            await AddRangeAndSaveAsync(user);
            DbContext.ChangeTracker.Clear();

            // Act
            await _repository.UpdateAsync(updatedUser);
            await DbContext.SaveChangesAsync();

            // Assert
            DbContext.Users.Should().Contain(updatedUser);
            DbContext.Users.First(t => t.Id == user.Id)
                .Should().BeEquivalentTo(updatedUser);
        }
    }
}
