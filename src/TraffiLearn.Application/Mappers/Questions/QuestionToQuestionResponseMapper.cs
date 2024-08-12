using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.Mapper.Questions
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
                Answers: source.Answers.ToList());
        }
    }
}
