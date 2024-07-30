using TraffiLearn.Domain.Errors.Tickets;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Ticket : Entity
    {
        private readonly List<Question> _questions = [];

        private Ticket(Guid id)
            : base(id)
        { }

        private Ticket(
            TicketId ticketId,
            TicketNumber ticketNumber)
            : base(ticketId.Value)
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

            if (!question.Tickets.Contains(this))
            {
                var addResult = question.AddTicket(this);

                if (addResult.IsFailure)
                {
                    return addResult.Error;
                }
            }

            return Result.Success();
        }

        public Result RemoveQuestion(Question question)
        {
            if (!_questions.Contains(question))
            {
                return TicketErrors.QuestionNotFound;
            }

            _questions.Remove(question);

            if (question.Tickets.Contains(this))
            {
                var removeResult = question.RemoveTicket(this);

                if (removeResult.IsFailure)
                {
                    return removeResult.Error;
                }
            }

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

        private static Result ValidateQuestions(List<Question> questions)
        {
            if (questions.Count == 0)
            {
                return TicketErrors.NoQuestions;
            }

            var uniqueQuestions = new HashSet<Question>(questions);

            if (uniqueQuestions.Count != questions.Count)
            {
                return TicketErrors.DuplicateQuestions;
            }

            return Result.Success();
        }
    }

    public sealed record TicketId(Guid Value);
}
