using FluentValidation;

namespace TraffiLearn.Application.Queries.Tickets.GetById
{
    internal sealed class GetTicketByIdQueryValidator
        : AbstractValidator<GetTicketByIdQuery>
    {
        public GetTicketByIdQueryValidator()
        {
            RuleFor(x => x.TicketId)
                .NotEmpty();
        }
    }
}
