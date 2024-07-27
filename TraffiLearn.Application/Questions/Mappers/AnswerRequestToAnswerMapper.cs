using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Answers.Request;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.Questions.Mappers
{
    internal sealed class AnswerRequestToAnswerMapper : Mapper<AnswerRequest, Answer>
    {
        public override Answer Map(AnswerRequest source)
        {
            return Answer.Create(
                text: source.Text,
                isCorrect: source.IsCorrect);
        }
    }
}
