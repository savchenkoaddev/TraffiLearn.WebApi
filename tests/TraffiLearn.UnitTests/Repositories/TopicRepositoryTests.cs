using FluentAssertions;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Infrastructure.Persistence.Repositories;
using TraffiLearn.Testing.Shared.Factories;
using TraffiLearn.UnitTests.Abstractions;

namespace TraffiLearn.UnitTests.Repositories
{
    public sealed class TopicRepositoryTests : BaseRepositoryTest
    {
        private readonly TopicRepository _repository;

        public TopicRepositoryTests()
        {
            _repository = new TopicRepository(DbContext);
        }

        [Fact]
        public async Task InsertAsync_ShouldAddTopic()
        {
            // Arrange
            var topic = TopicFixtureFactory.CreateTopic();

            // Act
            await _repository.InsertAsync(topic);
            await DbContext.SaveChangesAsync();

            // Assert
            DbContext.Topics.Should().Contain(topic);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteTopic()
        {
            // Arrange
            var topic = TopicFixtureFactory.CreateTopic();

            await AddRangeAndSaveAsync(topic);

            // Act
            await _repository.DeleteAsync(topic);
            await DbContext.SaveChangesAsync();

            // Assert
            DbContext.Topics.Should().NotContain(topic);
        }

        [Fact]
        public async Task ExistsAsync_IfTopicExists_ShouldReturnTrue()
        {
            // Arrange
            var topic = TopicFixtureFactory.CreateTopic();

            await AddRangeAndSaveAsync(topic);

            // Act
            var result = await _repository.ExistsAsync(topic.Id);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsAsync_IfTopicDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var topicId = new TopicId(Guid.NewGuid());

            // Act
            var result = await _repository.ExistsAsync(topicId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateTopic()
        {
            // Arrange
            var topic = TopicFixtureFactory.CreateTopic();

            var updatedTopic = Topic.Create(
                topic.Id,
                TopicNumber.Create(2).Value,
                TopicTitle.Create("Updated topic").Value)
                .Value;

            await AddRangeAndSaveAsync(topic);
            DbContext.ChangeTracker.Clear();

            // Act
            await _repository.UpdateAsync(updatedTopic);
            await DbContext.SaveChangesAsync();

            // Assert
            DbContext.Topics.Should().Contain(updatedTopic);
            DbContext.Topics.First(t => t.Id == updatedTopic.Id)
                .Should().BeEquivalentTo(updatedTopic);
        }

        [Fact]
        public async Task GetByIdAsync_IfTopicExists_ShouldReturnTopic()
        {
            // Arrange
            var topic = TopicFixtureFactory.CreateTopic();

            await AddRangeAndSaveAsync(topic);

            // Act
            var result = await _repository.GetByIdAsync(topic.Id);

            // Assert
            result.Should().Be(topic);
        }

        [Fact]
        public async Task GetByIdAsync_IfTopicDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var topicId = new TopicId(Guid.NewGuid());

            // Act
            var result = await _repository.GetByIdAsync(topicId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdWithQuestionsAsync_IfTopicExists_ShouldReturnTopicWithQuestions()
        {
            // Arrange
            var topic = TopicFixtureFactory.CreateTopic();

            var question = QuestionFixtureFactory.CreateQuestion();

            topic.AddQuestion(question);

            await AddRangeAndSaveAsync(topic, question);

            // Act
            var result = await _repository.GetByIdWithQuestionsAsync(topic.Id);

            // Assert
            result.Should().NotBeNull().And.Be(topic);
            result!.Questions.Should().Contain(question);
        }

        [Fact]
        public async Task GetByIdWithQuestionsAsync_IfTopicDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var topicId = new TopicId(Guid.NewGuid());

            // Act
            var result = await _repository.GetByIdWithQuestionsAsync(topicId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetManyByQuestionIdAsync_IfQuestionExists_ShouldReturnTopics()
        {
            // Arrange
            var topic1 = TopicFixtureFactory.CreateTopic(number: 1);

            var topic2 = TopicFixtureFactory.CreateTopic(number: 2);

            var question = QuestionFixtureFactory.CreateQuestion();

            topic1.AddQuestion(question);
            topic2.AddQuestion(question);

            await AddRangeAndSaveAsync(topic1, topic2, question);

            // Act
            var result = await _repository.GetManyByQuestionIdAsync(question.Id);

            // Assert
            result.Should().HaveCount(2);
            result.Should()
                .Contain(topic1)
                .And
                .Contain(topic2);
        }

        [Fact]
        public async Task GetManyByQuestionIdAsync_IfQuestionDoesNotExist_ShouldReturnEmptyCollection()
        {
            // Arrange
            var questionId = new QuestionId(Guid.NewGuid());

            // Act
            var result = await _repository.GetManyByQuestionIdAsync(questionId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetManyByQuestionIdAsync_IfQuestionHasNoTopics_ShouldReturnEmptyCollection()
        {
            // Arrange
            var question = QuestionFixtureFactory.CreateQuestion();

            await AddRangeAndSaveAsync(question);

            // Act
            var result = await _repository.GetManyByQuestionIdAsync(question.Id);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllSortedByNumberAsync_IfTopicsDoNotExist_ShouldReturnEmptyCollection()
        {
            // Arrange

            // Act
            var result = await _repository.GetAllSortedByNumberAsync();

            // Assert
            result.Should().BeEmpty();
        }
    }
}
