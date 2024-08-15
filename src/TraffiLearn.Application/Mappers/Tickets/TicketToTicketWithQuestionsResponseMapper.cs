using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Tickets;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.Mappers.Tickets
{
    internal sealed class TicketToTicketWithQuestionsResponseMapper 
        : Mapper<Ticket, TicketWithQuestionsResponse>
    {
        public override TicketWithQuestionsResponse Map(Ticket source)
        {
            return new TicketWithQuestionsResponse(
                TicketId: source.Id.Value,
                TicketNumber: source.TicketNumber.Value,
                Questions: source.Questions.ToList());
        }
    }
}
