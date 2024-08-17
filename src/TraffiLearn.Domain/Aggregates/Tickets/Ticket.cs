using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Tickets.Errors;
using TraffiLearn.Domain.Aggregates.Tickets.ValueObjects;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Tickets
{
    public sealed class Ticket : AggregateRoot<TicketId>
    {
        private readonly HashSet<QuestionId> _questionsIds = [];
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

        public IReadOnlyCollection<QuestionId> QuestionsIds => _questionsIds;

        public Result AddQuestion(QuestionId questionId)
        {
            if (_questionsIds.Contains(questionId))
            {
                return TicketErrors.QuestionAlreadyAdded;
            }

            _questionsIds.Add(questionId);

            return Result.Success();
        }

        public Result RemoveQuestion(QuestionId questionId)
        {
            if (!_questionsIds.Contains(questionId))
            {
                return TicketErrors.QuestionNotFound;
            }

            _questionsIds.Remove(questionId);

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
