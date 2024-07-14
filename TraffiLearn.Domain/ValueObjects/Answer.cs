using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Domain.ValueObjects
{
    public sealed record Answer
    {
        [StringLength(300)]
        public string Text { get; init; }

        public bool IsCorrect { get; init; }
    };
}
