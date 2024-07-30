using FluentValidation;

namespace TraffiLearn.Application.Queries.Tickets.GetQuestionsForTicket
{
    internal sealed class GetQuestionsForTicketQueryValidator 
        : AbstractValidator<GetQuestionsForTicketQuery>
    {
        public GetQuestionsForTicketQueryValidator()
        {
            RuleFor(x => x.TicketId)
                .NotEmpty();
        }
    }
}
