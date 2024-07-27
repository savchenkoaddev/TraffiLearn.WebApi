using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.QuestionTitleDetails.Response;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.Questions.Mappers
{
    internal sealed class QtdToQtdResponseMapper : Mapper<QuestionTitleDetails, QuestionTitleDetailsResponse>
    {
        public override QuestionTitleDetailsResponse Map(QuestionTitleDetails source)
        {
            return new QuestionTitleDetailsResponse(
                TicketNumber: source.TicketNumber,
                QuestionNumber: source.QuestionNumber);
        }
    }
}
