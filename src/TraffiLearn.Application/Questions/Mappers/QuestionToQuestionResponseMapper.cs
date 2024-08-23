using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Aggregates.Questions;

namespace TraffiLearn.Application.Questions.Mappers
{
    internal sealed class QuestionToQuestionResponseMapper : Mapper<Question, QuestionResponse>
    {
        public override QuestionResponse Map(Question source)
        {
            return new QuestionResponse(
                Id: source.Id.Value,
                Content: source.Content.Value,
                Explanation: source.Explanation.Value,
                ImageUri: source.ImageUri?.Value,
                LikesCount: source.LikesCount,
                DislikesCount: source.DislikesCount,
                QuestionNumber: source.QuestionNumber.Value,
                Answers: source.Answers.Select(a => new AnswerResponse(
                    a.Text,
                    a.IsCorrect)).ToList());
        }
    }
}
