using System.ComponentModel.DataAnnotations;

namespace TraffiLearn.Application.UseCases.Questions.Options
{
    public sealed class QuestionsSettings
    {
        public const string SectionName = nameof(QuestionsSettings);

        [Range(1, 50)]
        public int TheoryTestQuestionsCount { get; set; } = 20;
    }
}
