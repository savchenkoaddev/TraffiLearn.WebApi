using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TraffiLearn.Domain.ValueObjects
{
    [ComplexType]
    public sealed record QuestionTitleDetails
    {
        [Range(1, int.MaxValue)]
        public int? TicketNumber { get; init; }

        [Range(1, int.MaxValue)]
        public int? QuestionNumber { get; init; }
    };
}
