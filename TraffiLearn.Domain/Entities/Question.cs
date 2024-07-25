using System.ComponentModel.DataAnnotations;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Domain.Entities 
{
    public sealed class Question : Entity
    {
        public Question(Guid id) : base(id)
        { }

        [StringLength(2000)]
        public string Content { get; set; }

        public string Explanation { get; set; }

        [Range(0, int.MaxValue)]
        public int LikesCount { get; set; }

        [Range(0, int.MaxValue)]
        public int DislikesCount { get; set; }

        public ICollection<Topic> Topics { get; set; } = [];

        public List<Answer> Answers { get; set; } = [];

        public QuestionTitleDetails TitleDetails { get; set; }

        [StringLength(200)]
        public string? ImageUri { get; set; }
    }
}
