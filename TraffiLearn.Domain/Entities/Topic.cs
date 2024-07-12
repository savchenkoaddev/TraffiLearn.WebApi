using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Topic
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public int Number { get; set; }

        [StringLength(300)]
        public string Title { get; set; }

        public ICollection<Question> Questions { get; set; } = [];
    }
}
