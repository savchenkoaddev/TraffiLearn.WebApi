using MediatR;
using TraffiLearn.Application.Questions.Commands.Create;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Application.Questions.Queries.GetAll;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Testing.Shared.Factories;

namespace TraffiLearn.IntegrationTests.Questions
{
    internal sealed class QuestionTestHelper
    {
        private readonly ISender _sender;

        public QuestionTestHelper(ISender sender)
        {
            _sender = sender;
        }

        public async Task<Result> CreateValidQuestionAsync(List<Guid> TopicIds)
        {
            var validQuestion = QuestionFixtureFactory.CreateQuestion();

            return await _sender.Send(new CreateQuestionCommand(
                validQuestion.Content.Value,
                validQuestion.Explanation.Value,
                validQuestion.QuestionNumber.Value,
                TopicIds,
                ConvertAnswers(validQuestion)!,
                null));
        }

        public async Task<Guid> GetFirstQuestionIdAsync()
        {
            var questions = await GetAllQuestionsAsync();

            return questions.First().Id;
        }

        public async Task<IEnumerable<QuestionResponse>> GetAllQuestionsAsync()
        {
            return (await _sender.Send(new GetAllQuestionsQuery())).Value;
        }

        private static List<AnswerRequest> ConvertAnswers(Question validQuestion)
        {
            return validQuestion.Answers.Select(a => new AnswerRequest(a.Text, a.IsCorrect)).ToList();
        }
    }
}
