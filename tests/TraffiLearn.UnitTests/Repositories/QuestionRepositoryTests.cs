using FluentAssertions;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Tickets.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Infrastructure.Persistence.Repositories;
using TraffiLearn.Testing.Shared.Factories;
using TraffiLearn.UnitTests.Abstractions;

namespace TraffiLearn.UnitTests.Repositories
{
    public sealed class QuestionRepositoryTests : BaseRepositoryTest
    {
        private readonly QuestionRepository _repository;

        public QuestionRepositoryTests()
        {
            _repository = new QuestionRepository(DbContext);
        }

        [Fact]
        public async Task AddAsync_ShouldAddQuestion()
        {
            // Arrange
            var question = QuestionFixtureFactory.CreateQuestion();

            // Act
            await _repository.AddAsync(question);
            await DbContext.SaveChangesAsync();

            // Assert
            DbContext.Questions.Should().Contain(question);
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteQuestion()
        {
            // Arrange
            var question = QuestionFixtureFactory.CreateQuestion();

            await AddRangeAndSaveAsync(question);

            // Act
            await _repository.DeleteAsync(question);
            await DbContext.SaveChangesAsync();

            // Assert
            DbContext.Questions.Should().NotContain(question);
        }

        [Fact]
        public async Task ExistsAsync_IfQuestionExists_ShouldReturnTrue()
        {
            // Arrange
            var question = QuestionFixtureFactory.CreateQuestion();

            await AddRangeAndSaveAsync(question);

            // Act
            var result = await _repository.ExistsAsync(question.Id);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsAsync_IfQuestionDoesNotExist_ShouldReturnFalse()
        {
            // Arrange
            var questionId = new QuestionId(Guid.NewGuid());

            // Act
            var result = await _repository.ExistsAsync(questionId);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetByIdAsync_IfQuestionExists_ShouldReturnQuestion()
        {
            // Arrange
            var question = QuestionFixtureFactory.CreateQuestion();

            await AddRangeAndSaveAsync(question);

            // Act
            var result = await _repository.GetByIdAsync(question.Id);

            // Assert
            result.Should().Be(question);
        }

        [Fact]
        public async Task GetByIdAsync_IfQuestionDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var questionId = new QuestionId(Guid.NewGuid());

            // Act
            var result = await _repository.GetByIdAsync(questionId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdWithTicketsAsync_IfQuestionExists_ShouldReturnQuestionWithTickets()
        {
            // Arrange
            var question = QuestionFixtureFactory.CreateQuestion();

            var ticket1 = TicketFixtureFactory.CreateTicket(number: 1);

            var ticket2 = TicketFixtureFactory.CreateTicket(number: 2);

            question.AddTicket(ticket1);
            question.AddTicket(ticket2);

            await AddRangeAndSaveAsync(question, ticket1, ticket2);

            // Act
            var result = await _repository.GetByIdWithTicketsAsync(question.Id);

            // Assert
            result.Should().NotBeNull().And.Be(question);
            result!.Tickets.Should()
                .Contain(ticket1)
                .And
                .Contain(ticket2);
        }

        [Fact]
        public async Task GetByIdWithTicketsAsync_IfQuestionDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var questionId = new QuestionId(Guid.NewGuid());

            // Act
            var result = await _repository.GetByIdWithTicketsAsync(questionId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetByIdWithTopicsAsync_IfQuestionExists_ShouldReturnQuestionWithTopics()
        {
            // Arrange
            var question = QuestionFixtureFactory.CreateQuestion();

            var topic1 = TopicFixtureFactory.CreateTopic(number: 1);

            var topic2 = TopicFixtureFactory.CreateTopic(number: 2);

            question.AddTopic(topic1);
            question.AddTopic(topic2);

            await AddRangeAndSaveAsync(question, topic1, topic2);

            // Act
            var result = await _repository.GetByIdWithTopicsAsync(question.Id);

            // Assert
            result.Should().NotBeNull().And.Be(question);
            result!.Topics.Should()
            .Contain(topic1)
                .And
                .Contain(topic2);
        }

        [Fact]
        public async Task GetByIdWithTopicsAsync_IfQuestionDoesNotExist_ShouldReturnNull()
        {
            // Arrange
            var questionId = new QuestionId(Guid.NewGuid());

            // Act
            var result = await _repository.GetByIdWithTopicsAsync(questionId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetManyByTicketIdAsync_IfTicketExists_ShouldReturnQuestions()
        {
            // Arrange
            var ticket = TicketFixtureFactory.CreateTicket();

            var question1 = QuestionFixtureFactory.CreateQuestion(number: 1);

            var question2 = QuestionFixtureFactory.CreateQuestion(number: 2);

            ticket.AddQuestion(question1);
            ticket.AddQuestion(question2);

            await AddRangeAndSaveAsync(ticket, question1, question2);

            // Act
            var result = await _repository.GetManyByTicketIdAsync(ticket.Id);

            // Assert
            result.Should().HaveCount(2);
            result.Should()
                .Contain(question1)
                .And
                .Contain(question2);
        }

        [Fact]
        public async Task GetManyByTicketIdAsync_IfTicketDoesNotExist_ShouldReturnEmptyCollection()
        {
            // Arrange
            var ticketId = new TicketId(Guid.NewGuid());

            // Act
            var result = await _repository.GetManyByTicketIdAsync(ticketId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetManyByTicketIdAsync_IfTicketHasNoQuestions_ShouldReturnEmptyCollection()
        {
            // Arrange
            var ticket = TicketFixtureFactory.CreateTicket();

            await AddRangeAndSaveAsync(ticket);

            // Act
            var result = await _repository.GetManyByTicketIdAsync(ticket.Id);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetManyByTopicIdAsync_IfTopicExists_ShouldReturnQuestions()
        {
            // Arrange
            var topic = TopicFixtureFactory.CreateTopic();

            var question1 = QuestionFixtureFactory.CreateQuestion(number: 1);

            var question2 = QuestionFixtureFactory.CreateQuestion(number: 2);

            topic.AddQuestion(question1);
            topic.AddQuestion(question2);

            await AddRangeAndSaveAsync(topic, question1, question2);

            // Act
            var result = await _repository.GetManyByTopicIdAsync(topic.Id);

            // Assert
            result.Should().HaveCount(2);
            result.Should()
                .Contain(question1)
                .And
                .Contain(question2);
        }

        [Fact]
        public async Task GetManyByTopicIdAsync_IfTopicDoesNotExist_ShouldReturnEmptyCollection()
        {
            // Arrange
            var topicId = new TopicId(Guid.NewGuid());

            // Act
            var result = await _repository.GetManyByTopicIdAsync(topicId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetManyByTopicIdAsync_IfTopicHasNoQuestions_ShouldReturnEmptyCollection()
        {
            // Arrange
            var topic = TopicFixtureFactory.CreateTopic();

            await AddRangeAndSaveAsync(topic);

            // Act
            var result = await _repository.GetManyByTopicIdAsync(topic.Id);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetUserDislikedQuestionsAsync_IfUserExists_ShouldReturnQuestions()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            var question1 = QuestionFixtureFactory.CreateQuestion(number: 1);

            var question2 = QuestionFixtureFactory.CreateQuestion(number: 2);

            user.DislikeQuestion(question1);
            user.DislikeQuestion(question2);

            await AddRangeAndSaveAsync(user, question1, question2);

            // Act
            var result = await _repository.GetUserDislikedQuestionsAsync(user.Id);

            // Assert
            result.Should().HaveCount(2);
            result.Should()
                .Contain(question1)
                .And
                .Contain(question2);
        }

        [Fact]
        public async Task GetUserDislikedQuestionsAsync_IfUserDoesNotExist_ShouldReturnEmptyCollection()
        {
            // Arrange
            var userId = new UserId(Guid.NewGuid());

            // Act
            var result = await _repository.GetUserDislikedQuestionsAsync(userId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetUserLikedQuestionsAsync_IfUserExists_ShouldReturnQuestions()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            var question1 = QuestionFixtureFactory.CreateQuestion(number: 1);

            var question2 = QuestionFixtureFactory.CreateQuestion(number: 2);

            user.LikeQuestion(question1);
            user.LikeQuestion(question2);

            await AddRangeAndSaveAsync(user, question1, question2);

            // Act
            var result = await _repository.GetUserLikedQuestionsAsync(user.Id);

            // Assert
            result.Should().HaveCount(2);
            result.Should()
                .Contain(question1)
                .And
                .Contain(question2);
        }

        [Fact]
        public async Task GetUserLikedQuestionsAsync_IfUserDoesNotExist_ShouldReturnEmptyCollection()
        {
            // Arrange
            var userId = new UserId(Guid.NewGuid());

            // Act
            var result = await _repository.GetUserLikedQuestionsAsync(userId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetUserMarkedQuestionsAsync_IfUserExists_ShouldReturnQuestions()
        {
            // Arrange
            var user = UserFixtureFactory.CreateUser();

            var question1 = QuestionFixtureFactory.CreateQuestion(number: 1);

            var question2 = QuestionFixtureFactory.CreateQuestion(number: 2);

            user.MarkQuestion(question1);
            user.MarkQuestion(question2);

            await AddRangeAndSaveAsync(user, question1, question2);

            // Act
            var result = await _repository.GetUserMarkedQuestionsAsync(user.Id);

            // Assert
            result.Should().HaveCount(2);
            result.Should()
                .Contain(question1)
                .And
                .Contain(question2);
        }

        [Fact]
        public async Task GetUserMarkedQuestionsAsync_IfUserDoesNotExist_ShouldReturnEmptyCollection()
        {
            // Arrange
            var userId = new UserId(Guid.NewGuid());

            // Act
            var result = await _repository.GetUserMarkedQuestionsAsync(userId);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_IfPassedInvalidPage_ShouldThrowArgumentOutOfRangeException()
        {
            Func<Task> negativePageAction = async () =>
            {
                await _repository.GetAllAsync(
                    page: -1,
                    pageSize: 10);
            };

            Func<Task> zeroPageAction = async () =>
            {
                await _repository.GetAllAsync(
                    page: 0,
                    pageSize: 10);
            };

            await negativePageAction.Should().ThrowAsync<ArgumentOutOfRangeException>();
            await zeroPageAction.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }

        [Fact]
        public async Task GetAllAsync_IfPassedInvalidPageSize_ShouldThrowArgumentOutOfRangeException()
        {
            Func<Task> negativePageSizeAction = async () =>
            {
                await _repository.GetAllAsync(
                    page: 10,
                    pageSize: -1);
            };

            Func<Task> zeroPageSizeAction = async () =>
            {
                await _repository.GetAllAsync(
                    page: 10,
                    pageSize: 0);
            };

            await negativePageSizeAction.Should().ThrowAsync<ArgumentOutOfRangeException>();
            await zeroPageSizeAction.Should().ThrowAsync<ArgumentOutOfRangeException>();
        }

        [Fact]
        public async Task GetAllAsync_IfPassedValidPageAndPageSize_ShouldReturnPaginatedQuestions()
        {
            // Arrange
            var question1 = QuestionFixtureFactory.CreateQuestion(number: 1);
            var question2 = QuestionFixtureFactory.CreateQuestion(number: 2);
            var question3 = QuestionFixtureFactory.CreateQuestion(number: 3);
            await AddRangeAndSaveAsync(question1, question2, question3);

            // Act
            var result = await _repository.GetAllAsync(
                page: 1, 
                pageSize: 2);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(question1);
            result.Should().Contain(question2);
        }

        [Fact]
        public async Task GetAllAsync_IfPageExceedsAvailableData_ShouldReturnEmptyCollection()
        {
            // Arrange
            var question1 = QuestionFixtureFactory.CreateQuestion(number: 1);
            await AddRangeAndSaveAsync(question1);

            // Act
            var result = await _repository.GetAllAsync(
                page: 2, 
                pageSize: 1);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllAsync_IfPageSizeExceedsTotalQuestions_ShouldReturnAllAvailableQuestions()
        {
            // Arrange
            var question1 = QuestionFixtureFactory.CreateQuestion(number: 1);
            var question2 = QuestionFixtureFactory.CreateQuestion(number: 2);
            await AddRangeAndSaveAsync(question1, question2);

            // Act
            var result = await _repository.GetAllAsync(page: 1, pageSize: 10);

            // Assert
            result.Should().HaveCount(2);
            result.Should().Contain(question1);
            result.Should().Contain(question2);
        }

        [Fact]
        public async Task GetAllAsync_IfOnlyOneQuestionExists_ShouldReturnSingleQuestion()
        {
            // Arrange
            var question = QuestionFixtureFactory.CreateQuestion(number: 1);
            await AddRangeAndSaveAsync(question);

            // Act
            var result = await _repository.GetAllAsync(
                page: 1, 
                pageSize: 1);

            // Assert
            result.Should().HaveCount(1);
            result.Should().Contain(question);
        }

        [Fact]
        public async Task GetAllAsync_IfValidPagesRequested_ShouldReturnCorrectDataForEachPage()
        {
            // Arrange
            var question1 = QuestionFixtureFactory.CreateQuestion(number: 1);
            var question2 = QuestionFixtureFactory.CreateQuestion(number: 2);
            var question3 = QuestionFixtureFactory.CreateQuestion(number: 3);

            await AddRangeAndSaveAsync(question1, question2, question3);

            // Act
            var page1Result = await _repository.GetAllAsync(
                page: 1, 
                pageSize: 2);

            var page2Result = await _repository.GetAllAsync(
                page: 2, 
                pageSize: 2);

            // Assert
            page1Result.Should().Contain(question1).And.Contain(question2);
            page2Result.Should().Contain(question3);
        }
    }
}
