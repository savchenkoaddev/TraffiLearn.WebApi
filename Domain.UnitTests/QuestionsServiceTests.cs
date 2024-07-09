using FluentAssertions;
using Moq;
using TraffiLearn.Application.Services;
using TraffiLearn.Domain.RepositoryContracts;

namespace Domain.UnitTests
{
    public class QuestionsServiceTests
    {
        private readonly QuestionsService _service;
        private readonly Mock<IQuestionsRepository> _questionsRepositoryMock = new Mock<IQuestionsRepository>();

        public QuestionsServiceTests()
        {
            _service = new QuestionsService(_questionsRepositoryMock.Object);
        }

        [Fact]
        public async void AddAsync_ShouldThrowArgumentNullException_WhenQuestionNull()
        {
            Func<Task> action = async () =>
            {
                await _service.AddAsync(null);
            };

            await action.Should().ThrowAsync<ArgumentNullException>();
        }
    }
}