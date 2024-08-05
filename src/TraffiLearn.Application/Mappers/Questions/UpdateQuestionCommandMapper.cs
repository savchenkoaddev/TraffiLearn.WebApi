using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Commands.Questions.Update;
using TraffiLearn.Application.DTO.Answers;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;

namespace TraffiLearn.Application.Mapper.Questions
{
    internal sealed class UpdateQuestionCommandMapper
        : Mapper<UpdateQuestionCommand, Result<Question>>
    {
        public override Result<Question> Map(UpdateQuestionCommand source)
        {
            var answersResult = ParseAnswers(source.Answers);

            if (answersResult.IsFailure)
            {
                return Result.Failure<Question>(answersResult.Error);
            }

            var answers = answersResult.Value;

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

            Result<QuestionNumber> questionNumberResult = QuestionNumber.Create(source.QuestionNumber.Value);

            if (questionNumberResult.IsFailure)
            {
                return Result.Failure<Question>(questionNumberResult.Error);
            }

            return Question.Create(
                Guid.NewGuid(),
                contentResult.Value,
                explanationResult.Value,
                questionNumberResult.Value,
                answers,
                null);
        }

        private Result<List<Answer>> ParseAnswers(IEnumerable<AnswerRequest?> requestAnswers)
        {
            List<Answer> answers = [];

            foreach (var answer in requestAnswers)
            {
                var answerCreateResult = Answer.Create(
                    text: answer.Text,
                    isCorrect: answer.IsCorrect.Value);

                if (answerCreateResult.IsFailure)
                {
                    return Result.Failure<List<Answer>>(answerCreateResult.Error);
                }

                answers.Add(answerCreateResult.Value);
            }

            return Result.Success(answers);
        }
    }
}
