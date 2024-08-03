using TraffiLearn.Domain.Errors.Tickets;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Tickets;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Ticket : Entity
    {
        private readonly List<Question> _questions = [];

        private Ticket(Guid id)
            : base(id)
        { }

        private Ticket(
            Guid ticketId,
            TicketNumber ticketNumber)
            : base(ticketId)
        {
            TicketNumber = ticketNumber;
        }

        public TicketNumber TicketNumber { get; private set; }

        public IReadOnlyCollection<Question> Questions => _questions;

        public Result AddQuestion(Question question)
        {
            if (_questions.Contains(question))
            {
                return TicketErrors.QuestionAlreadyAdded;
            }

            _questions.Add(question);

            return Result.Success();
        }

        public Result RemoveQuestion(Question question)
        {
            if (!_questions.Contains(question))
            {
                return TicketErrors.QuestionNotFound;
            }

            _questions.Remove(question);

            return Result.Success();
        }

        public Result Update(TicketNumber ticketNumber)
        {
            TicketNumber = ticketNumber;

            return Result.Success();
        }

        public static Result<Ticket> Create(
            Guid ticketId,
            TicketNumber ticketNumber)
        {
            return new Ticket(
                ticketId,
                ticketNumber);
        }
    }
}
