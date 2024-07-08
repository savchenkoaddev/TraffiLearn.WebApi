namespace TraffiLearn.Domain.Entities
{
    public sealed class Question
    {
        public string Text { get; set; }
        public List<string> PossibleAnswears { get; set; }
        public List<string> CorrectAnswears { get; set; }
        public QuestionNumberDetails NumberDetails { get; set; }
    }

    public sealed record QuestionNumberDetails(int TickerNumber, int QuestionNumber);
}
