using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Answers.Response;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.Questions.Mappers
{
    internal sealed class AnswerToAnswerResponseMapper : Mapper<Answer, AnswerResponse>
    {
        public override AnswerResponse Map(Answer source)
        {
            return new AnswerResponse(
                Text: source.Text,
                IsCorrect: source.IsCorrect);
        }
    }
}
