using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.Answers;
using TraffiLearn.Domain.Aggregates.Questions.QuestionContents;
using TraffiLearn.Domain.Aggregates.Questions.QuestionExplanations;
using TraffiLearn.Domain.Aggregates.Questions.QuestionNumbers;
using TraffiLearn.Domain.Common.ImageUris;

namespace TraffiLearn.Testing.Shared.Factories
{
    public static class QuestionFixtureFactory
    {
        public static Question CreateQuestion(string content = "value",
            string explanation = "value",
            int number = QuestionNumber.MinValue,
            string imageUri = "http://127.0.0.1:10000/devstoreaccount1")
        {
            return Question.Create(
                new QuestionId(Guid.NewGuid()),
                CreateContent(content),
                CreateExplanation(explanation),
                CreateNumber(number),
                CreateAnswers(),
                CreateImageUri(imageUri)).Value;
        }

        public static QuestionContent CreateContent(string content = "value")
        {
            return QuestionContent.Create(content).Value;
        }

        public static QuestionExplanation CreateExplanation(string explanantion = "value")
        {
            return QuestionExplanation.Create(explanantion).Value;
        }

        public static QuestionNumber CreateNumber(int number = QuestionNumber.MinValue)
        {
            return QuestionNumber.Create(number).Value;
        }

        public static Answer CreateAnswer()
        {
            return Answer.Create("new-answer", true).Value;
        }

        public static List<Answer> CreateAnswers()
        {
            return [
                Answer.Create("text1", true).Value,
                Answer.Create("text2", false).Value
            ];
        }

        public static ImageUri CreateImageUri(string imageUri = "http://127.0.0.1:10000/devstoreaccount1")
        {
            return ImageUri.Create(imageUri).Value;
        }
    }
}
