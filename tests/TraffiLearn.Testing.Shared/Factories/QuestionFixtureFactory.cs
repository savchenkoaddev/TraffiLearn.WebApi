﻿using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;

namespace TraffiLearn.Testing.Shared.Factories
{
    public static class QuestionFixtureFactory
    {
        public static Question CreateQuestion()
        {
            return Question.Create(
                new QuestionId(Guid.NewGuid()),
                CreateContent(),
                CreateExplanation(),
                CreateNumber(),
                CreateAnswers(),
                CreateImageUri()).Value;
        }

        public static QuestionContent CreateContent()
        {
            return QuestionContent.Create("value").Value;
        }

        public static QuestionExplanation CreateExplanation()
        {
            return QuestionExplanation.Create("value").Value;
        }

        public static QuestionNumber CreateNumber()
        {
            return QuestionNumber.Create(QuestionNumber.MinValue).Value;
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

        public static ImageUri CreateImageUri()
        {
            return ImageUri.Create("http://127.0.0.1:10000/devstoreaccount1").Value;
        }
    }
}