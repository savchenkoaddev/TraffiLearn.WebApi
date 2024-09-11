using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Aggregates.Questions;

namespace TraffiLearn.Application.Questions.Mappers
{
    internal sealed class QuestionToQuestionResponseMapper
        : Mapper<Question, QuestionResponse>
    {
        public override QuestionResponse Map(Question source)
        {
            string? explanation = null;

            if (source.Explanation is not null)
            {
                explanation = source.Explanation.Value;
            }

            return new QuestionResponse(
                Id: source.Id.Value,
                Content: source.Content.Value,
                Explanation: explanation,
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
