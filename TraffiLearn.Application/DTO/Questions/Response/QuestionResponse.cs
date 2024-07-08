using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.DTO.Questions.Response
{
    public sealed class QuestionResponse
    {
        public Guid Id { get; set; }

        public string? Text { get; set; }

        public List<string>? PossibleAnswears { get; set; }

        public List<string>? CorrectAnswears { get; set; }

        public QuestionNumberDetails? NumberDetails { get; set; }
    }
}
