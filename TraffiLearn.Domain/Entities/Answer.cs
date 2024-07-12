using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Answer
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Question Question { get; set; }

        [StringLength(300)]
        public string Text { get; set; }

        public bool IsCorrect { get; set; }
    }
}
