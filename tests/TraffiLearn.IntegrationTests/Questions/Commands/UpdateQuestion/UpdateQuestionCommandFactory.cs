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

        public List<UpdateQuestionCommand> CreateInvalidCommands(
            Guid questionId,
            List<Guid>? topicIds,
            IFormFile? image = null,
            bool? removeOldImageIfNewImageMissing = true)
        {
            var command = CreateValidCommand(
                questionId,
                topicIds,
                image,
                removeOldImageIfNewImageMissing);

            return [
                command with { QuestionId = null },
                command with { Content = null },
                command with { Content = " " },
                command with { Explanation = null },
                command with { Explanation = " " },
                command with { QuestionNumber = -1 },
                command with { QuestionNumber = 0 },
                command with { Answers = null },
                command with { Answers = [] },

                command with { Answers = [
                    new AnswerRequest(
                        Text: null,
                        IsCorrect: true)
                ] },

                command with { Answers = [
                    new AnswerRequest(
                        Text: " ",
                        IsCorrect: true)
                ] },

                command with { Answers = [
                    new AnswerRequest(
                        Text: "Content",
                        IsCorrect: false)
                ] },

                command with { Answers = [
                    new AnswerRequest(
                        Text: "Content",
                        IsCorrect: false),
                    new AnswerRequest(
                        Text: "Content",
                        IsCorrect: true)
                ] },

                command with { Answers = [
                    new AnswerRequest(
                        Text: "Content",
                        IsCorrect: false),
                    new AnswerRequest(
                        Text: "Content1",
                        IsCorrect: false)
                ] },

                command with { TopicIds = null },
                command with { TopicIds = [] }
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
