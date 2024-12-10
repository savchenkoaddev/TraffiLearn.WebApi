using FluentValidation;

namespace TraffiLearn.Application.UseCases.Tickets.Queries.GetById
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
