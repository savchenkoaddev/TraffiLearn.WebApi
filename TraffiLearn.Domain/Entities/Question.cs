using System.ComponentModel.DataAnnotations;
using TraffiLearn.Domain.ValueObjects;

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

        public ICollection<DrivingCategory> DrivingCategories { get; set; }
    }
}
