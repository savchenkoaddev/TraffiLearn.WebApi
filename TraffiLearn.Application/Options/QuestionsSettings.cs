namespace TraffiLearn.Application.Options
{
    public sealed class QuestionsSettings
    {
        public const string SectionName = nameof(QuestionsSettings);

        public int? TheoryTestQuestionsCount { get; set; } = 20;

        public bool? DemandEnoughRecordsOnTheoryTestFetching { get; set; } = true;
    }
}
