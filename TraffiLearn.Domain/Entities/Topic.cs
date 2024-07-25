using System.ComponentModel.DataAnnotations;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Entities
{
    public sealed class Topic : Entity
    {
        public Topic(Guid id) : base(id)
        { }

        public int Number { get; set; }

        [StringLength(300)]
        public string Title { get; set; }

        public ICollection<Question> Questions { get; set; } = [];
    }
}
