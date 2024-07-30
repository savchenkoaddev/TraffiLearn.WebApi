using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.Commands.Questions.Create
{
    internal sealed class CreateQuestionCommandMapper : Mapper<CreateQuestionCommand, Result<Question>>
    {
        public override Result<Question> Map(CreateQuestionCommand source)
        {
            var questionId = new QuestionId(Guid.NewGuid());

            List<Answer> answers = [];

            foreach (var answer in source.Answers)
            {
                Result<Answer> answerResult = Answer.Create(answer.Text, answer.IsCorrect.Value);

                if (answerResult.IsFailure)
                {
                    return Result.Failure<Question>(answerResult.Error);
                }

                answers.Add(answerResult.Value);
            }

            Result<QuestionContent> contentResult = QuestionContent.Create(source.Content);

            if (contentResult.IsFailure)
            {
                return Result.Failure<Question>(contentResult.Error);
            }

            Result<QuestionExplanation> explanationResult = QuestionExplanation.Create(source.Explanation);

            if (explanationResult.IsFailure)
            {
                return Result.Failure<Question>(explanationResult.Error);
            }

            Result<Domain.ValueObjects.TicketNumber> ticketNumberResult = Domain.ValueObjects.TicketNumber.Create(source.TicketNumber.Value);

            if (ticketNumberResult.IsFailure)
            {
                return Result.Failure<Question>(ticketNumberResult.Error);
            }

            Result<QuestionNumber> questionNumberResult = QuestionNumber.Create(source.QuestionNumber.Value);

            if (questionNumberResult.IsFailure)
            {
                return Result.Failure<Question>(questionNumberResult.Error);
            }

            return Question.Create(
                questionId,
                contentResult.Value,
                explanationResult.Value,
                ticketNumberResult.Value,
                questionNumberResult.Value,
                answers: answers,
                imageUri: null);
        }
    }
}
