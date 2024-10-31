using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.Questions.Commands.Create;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Aggregates.Questions.Answers;
using TraffiLearn.Domain.Aggregates.Questions.QuestionContents;
using TraffiLearn.Domain.Aggregates.Questions.QuestionExplanations;
using TraffiLearn.IntegrationTests.Topics;
using TraffiLearn.Testing.Shared.Factories;

namespace TraffiLearn.IntegrationTests.Questions.Commands.CreateQuestion
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
            var answers = ConvertAnswers(QuestionFixtureFactory.CreateAnswers());

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

        public IEnumerable<CreateQuestionCommand> CreateInvalidCommands(List<Guid>? topicIds = null)
        {
            var answers = ConvertAnswers(QuestionFixtureFactory.CreateAnswers());

            return [
                new CreateQuestionCommand(
                    Content: null,
                    Explanation: QuestionFixtureFactory.CreateExplanation().Value,
                    QuestionNumber: QuestionFixtureFactory.CreateNumber().Value,
                    TopicIds: [Guid.NewGuid()],
                    Answers: answers,
                    Image: null),

                new CreateQuestionCommand(
                    Content: " ",
                    Explanation: QuestionFixtureFactory.CreateExplanation().Value,
                    QuestionNumber: QuestionFixtureFactory.CreateNumber().Value,
                    TopicIds: [Guid.NewGuid()],
                    Answers: answers,
                    Image: null),

                new CreateQuestionCommand(
                    Content: new string('1', QuestionContent.MaxLength + 1),
                    Explanation: QuestionFixtureFactory.CreateExplanation().Value,
                    QuestionNumber: QuestionFixtureFactory.CreateNumber().Value,
                    TopicIds: [Guid.NewGuid()],
                    Answers: answers,
                    Image: null),

                new CreateQuestionCommand(
                    Content: QuestionFixtureFactory.CreateContent().Value,
                    Explanation: new string('1', QuestionExplanation.MaxLength + 1),
                    QuestionNumber: QuestionFixtureFactory.CreateNumber().Value,
                    TopicIds: [Guid.NewGuid()],
                    Answers: answers,
                    Image: null),

                new CreateQuestionCommand(
                    Content: QuestionFixtureFactory.CreateContent().Value,
                    Explanation: QuestionFixtureFactory.CreateExplanation().Value,
                    QuestionNumber: null,
                    TopicIds: [Guid.NewGuid()],
                    Answers: answers,
                    Image: null),

                new CreateQuestionCommand(
                    Content: QuestionFixtureFactory.CreateContent().Value,
                    Explanation: QuestionFixtureFactory.CreateExplanation().Value,
                    QuestionNumber: -1,
                    TopicIds: [Guid.NewGuid()],
                    Answers: answers,
                    Image: null),

                new CreateQuestionCommand(
                    Content: QuestionFixtureFactory.CreateContent().Value,
                    Explanation: QuestionFixtureFactory.CreateExplanation().Value,
                    QuestionNumber: QuestionFixtureFactory.CreateNumber().Value,
                    TopicIds: null,
                    Answers: answers,
                    Image: null),

                new CreateQuestionCommand(
                    Content: QuestionFixtureFactory.CreateContent().Value,
                    Explanation: QuestionFixtureFactory.CreateExplanation().Value,
                    QuestionNumber: QuestionFixtureFactory.CreateNumber().Value,
                    TopicIds: [],
                    Answers: answers,
                    Image: null),

                new CreateQuestionCommand(
                    Content: QuestionFixtureFactory.CreateContent().Value,
                    Explanation: QuestionFixtureFactory.CreateExplanation().Value,
                    QuestionNumber: QuestionFixtureFactory.CreateNumber().Value,
                    TopicIds: [Guid.NewGuid()],
                    Answers: null,
                    Image: null),

                new CreateQuestionCommand(
                    Content: QuestionFixtureFactory.CreateContent().Value,
                    Explanation: QuestionFixtureFactory.CreateExplanation().Value,
                    QuestionNumber: QuestionFixtureFactory.CreateNumber().Value,
                    TopicIds: [Guid.NewGuid()],
                    Answers: [],
                    Image: null)
            ];
        }

        private List<AnswerRequest> ConvertAnswers(List<Answer> answers)
        {
            return answers
                .Select(answer => new AnswerRequest(
                    Text: answer.Text,
                    IsCorrect: answer.IsCorrect))
                .ToList();
        }
    }
}
