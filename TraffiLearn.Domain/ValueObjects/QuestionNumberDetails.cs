using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraffiLearn.Domain.ValueObjects
{
    [ComplexType]
    public sealed record QuestionNumberDetails
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int? TicketNumber { get; init; }

        [Required]
        [Range(1, int.MaxValue)]
        public int? QuestionNumber { get; init; }
    };
}
