using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.ValueObjects.Questions;

namespace TraffiLearn.DomainTests.Factories
{
    internal sealed class QuestionFixtureFactory
    {
        public static Question CreateValidQuestion()
        {
            return Question.Create(
                new QuestionId(Guid.NewGuid()),
                CreateValidContent(),
                CreateValidExplanation(),
                CreateValidNumber(),
                CreateValidAnswers(),
                CreateValidImageUri()).Value;
        }

        public static QuestionContent CreateValidContent()
        {
            return QuestionContent.Create("value").Value;
        }

        public static QuestionExplanation CreateValidExplanation()
        {
            return QuestionExplanation.Create("value").Value;
        }

        public static QuestionNumber CreateValidNumber()
        {
            return QuestionNumber.Create(QuestionNumber.MinValue).Value;
        }

        public static List<Answer> CreateValidAnswers()
        {
            return [
                Answer.Create("text1", true).Value,
                Answer.Create("text2", false).Value
            ];
        }

        public static ImageUri CreateValidImageUri()
        {
            return ImageUri.Create("http://127.0.0.1:10000/devstoreaccount1").Value;
        }
    }
}
