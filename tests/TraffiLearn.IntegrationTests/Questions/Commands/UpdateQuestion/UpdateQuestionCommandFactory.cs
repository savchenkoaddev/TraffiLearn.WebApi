using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.Questions.Commands.Update;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Testing.Shared.Factories;

namespace TraffiLearn.IntegrationTests.Questions.Commands.UpdateQuestion
{
    public sealed class UpdateQuestionCommandFactory
    {
        public UpdateQuestionCommand CreateValidCommand(
            Guid questionId,
            List<Guid>? topicIds,
            IFormFile? image = null,
            bool? removeOldImageIfNewImageMissing = true)
        {
            return new UpdateQuestionCommand(
                QuestionId: questionId,
                Content: QuestionFixtureFactory.CreateContent().Value,
                Explanation: QuestionFixtureFactory.CreateExplanation().Value,
                QuestionNumber: QuestionFixtureFactory.CreateNumber().Value,
                Answers: ConvertAnswers(QuestionFixtureFactory.CreateAnswers()),
                TopicIds: topicIds,
                Image: image,
                removeOldImageIfNewImageMissing);
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
