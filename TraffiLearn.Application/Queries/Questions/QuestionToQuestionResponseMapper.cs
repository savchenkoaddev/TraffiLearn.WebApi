using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.Queries.Questions
{
    internal class QuestionToQuestionResponseMapper : Mapper<Question, QuestionResponse>
    {
        public override QuestionResponse Map(Question source)
        {
            return new QuestionResponse(
                Id: source.Id,
                Content: source.Content,
                Explanation: source.Explanation,
                ImageUri: source.ImageUri,
                LikesCount: source.LikesCount,
                DislikesCount: source.DislikesCount,
                TicketNumber: source.TicketNumber,
                QuestionNumber: source.QuestionNumber,
                Answers: source.Answers.ToList());
        }
    }
}
