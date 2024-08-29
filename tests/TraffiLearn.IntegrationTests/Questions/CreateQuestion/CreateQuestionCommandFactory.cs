using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.Questions.Commands.Create;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.IntegrationTests.Topics;
using TraffiLearn.Testing.Shared.Factories;

namespace TraffiLearn.IntegrationTests.Questions.CreateQuestion
{
    public sealed class CreateQuestionCommandFactory
    {
        private readonly ApiTopicClient _apiTopicClient;

        public CreateQuestionCommandFactory(ApiTopicClient apiTopicClient)
        {
            _apiTopicClient = apiTopicClient;
        }

        public CreateQuestionCommand CreateValidCommand(
            List<Guid> topicIds,
            IFormFile? image = null)
        {
            var answers = QuestionFixtureFactory.CreateAnswers()
                .Select(answer => new AnswerRequest(
                    Text: answer.Text,
                    IsCorrect: answer.IsCorrect))
                .ToList();

            return new CreateQuestionCommand(
                Content: QuestionFixtureFactory.CreateContent().Value,
                Explanation: QuestionFixtureFactory.CreateExplanation().Value,
                QuestionNumber: QuestionFixtureFactory.CreateNumber().Value,
                TopicIds: topicIds,
                Answers: answers,
                Image: image);
        }

        public async Task<CreateQuestionCommand> CreateValidCommandWithTopicAsync(
            IFormFile? image = null)
        {
            var topicId = await _apiTopicClient.CreateValidTopicAsAuthorizedAsync();

            return CreateValidCommand(
                topicIds: [topicId],
                image: image);
        }
    }
}
