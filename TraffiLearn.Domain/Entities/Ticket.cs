using TraffiLearn.Domain.Errors.Questions;
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
            TicketNumber ticketNumber,
            List<Question> questions)
            : base(ticketId.Value)
        {
            TicketNumber = ticketNumber;
            _questions = questions;
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

            if (question.Ticket != this)
            {
                var result = question.SetTicket(this);

                if (result.IsFailure)
                {
                    return result.Error;
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

            if (question.Ticket is not null)
            {
                var removeResult = question.RemoveTicket();

                if (removeResult.IsFailure)
                {
                    return removeResult.Error;
                }
            }

            return Result.Success();
        }

        public static Result<Ticket> Create(
            TicketId ticketId,
            TicketNumber ticketNumber,
            List<Question> questions)
        {
            var validationResult = ValidateQuestions(questions);

            if (validationResult.IsFailure)
            {
                return Result.Failure<Ticket>(validationResult.Error);
            }

            return new Ticket(
                ticketId,
                ticketNumber,
                questions);
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
