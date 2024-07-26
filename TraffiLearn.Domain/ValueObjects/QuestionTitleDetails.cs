namespace TraffiLearn.Domain.ValueObjects
{
    public sealed record QuestionTitleDetails
    {
        private QuestionTitleDetails(
            int ticketNumber,
            int questionNumber)
        {
            TicketNumber = ticketNumber;
            QuestionNumber = questionNumber;
        }

        public int TicketNumber { get; init; }

        public int QuestionNumber { get; init; }

        public static QuestionTitleDetails Create(
            int? ticketNumber,
            int? questionNumber)
        {
            ArgumentNullException.ThrowIfNull(ticketNumber, nameof(ticketNumber));
            ArgumentNullException.ThrowIfNull(questionNumber, nameof(questionNumber));

            if (ticketNumber < 1)
            {
                throw new ArgumentException("Ticket number cannot be less than one.");
            }

            if (questionNumber < 1)
            {
                throw new ArgumentException("Question number cannot be less than one");
            }

            return new QuestionTitleDetails(
                ticketNumber.Value,
                questionNumber.Value);
        }
    };
}
