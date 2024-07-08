using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Question
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [StringLength(500)]
        public string Text { get; set; }

        public List<string> PossibleAnswears { get; set; }

        public List<string> CorrectAnswears { get; set; }

        public QuestionNumberDetails NumberDetails { get; set; }
    }

    [ComplexType]
    public sealed record QuestionNumberDetails(int TickerNumber, int QuestionNumber);
}
