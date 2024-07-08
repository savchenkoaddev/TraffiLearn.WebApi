using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Application.DTO.Questions.Request
{
    public sealed class QuestionDeleteRequest
    {
        [Required]
        public Guid? Id { get; set; }
    }
}
