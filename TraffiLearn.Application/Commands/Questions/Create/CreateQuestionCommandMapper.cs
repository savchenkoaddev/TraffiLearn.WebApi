﻿using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.Commands.Questions.Create
{
    internal sealed class CreateQuestionCommandMapper : Mapper<CreateQuestionCommand, Question>
    {
        public override Question Map(CreateQuestionCommand source)
        {
            var questionId = new QuestionId(Guid.NewGuid());

            var answers = source.Answers.Select(x => Answer.Create(x.Text, x.IsCorrect.Value)).ToList();

            return Question.Create(
                questionId,
                QuestionContent.Create(source.Content),
                QuestionExplanation.Create(source.Explanation),
                TicketNumber.Create(source.TicketNumber.Value),
                QuestionNumber.Create(source.QuestionNumber.Value),
                answers: answers,
                imageUri: null);
        }
    }
}