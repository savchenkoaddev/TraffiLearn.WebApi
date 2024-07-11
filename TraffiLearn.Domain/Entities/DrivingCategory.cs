using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Domain.Entities
{
    public sealed class DrivingCategory
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [StringLength(20)]
        public string Code { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        public ICollection<Question> Questions { get; set; }
    }
}
