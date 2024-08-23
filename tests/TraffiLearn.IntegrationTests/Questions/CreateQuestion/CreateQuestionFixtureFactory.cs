using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.Questions.Commands.Create;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Testing.Shared.Factories;

namespace TraffiLearn.IntegrationTests.Questions.CreateQuestion
{
    internal sealed class CreateQuestionFixtureFactory
    {
        public static CreateQuestionCommand CreateValidCommand(
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
    }
}
