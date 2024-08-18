using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Tickets.Errors;
using TraffiLearn.Domain.Aggregates.Tickets.ValueObjects;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Tickets
{
    public sealed class Ticket : AggregateRoot<TicketId>
    {
        private readonly HashSet<Question> _questions = [];
        private TicketNumber _ticketNumber;

        private Ticket()
            : base(new(Guid.Empty))
        { }

        private Ticket(
            TicketId ticketId,
            TicketNumber ticketNumber)
            : base(ticketId)
        {
            TicketNumber = ticketNumber;
        }

        public TicketNumber TicketNumber
        {
            get
            {
                return _ticketNumber;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value, nameof(value));

                _ticketNumber = value;
            }
        }

        public IReadOnlyCollection<Question> Questions => _questions;

        public Result AddQuestion(Question question)
        {
            ArgumentNullException.ThrowIfNull(question, nameof(question));

            if (_questions.Contains(question))
            {
                return TicketErrors.QuestionAlreadyAdded;
            }

            _questions.Add(question);

            return Result.Success();
        }

        public Result RemoveQuestion(Question question)
        {
            ArgumentNullException.ThrowIfNull(question, nameof(question));

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
            TicketId ticketId,
            TicketNumber ticketNumber)
        {
            return new Ticket(
                ticketId,
                ticketNumber);
        }
    }
}
