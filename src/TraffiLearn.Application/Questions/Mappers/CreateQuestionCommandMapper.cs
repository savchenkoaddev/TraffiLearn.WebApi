using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Questions.Commands.Create;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Aggregates.Questions;
using TraffiLearn.Domain.Aggregates.Questions.Answers;
using TraffiLearn.Domain.Aggregates.Questions.QuestionContents;
using TraffiLearn.Domain.Aggregates.Questions.QuestionExplanations;
using TraffiLearn.Domain.Aggregates.Questions.QuestionNumbers;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Mappers
{
    internal sealed class CreateQuestionCommandMapper
        : Mapper<CreateQuestionCommand, Result<Question>>
    {
        public override Result<Question> Map(CreateQuestionCommand source)
        {
            QuestionId questionId = new(Guid.NewGuid());

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

            QuestionExplanation? explanation = null;

            if (source.Explanation is not null)
            {
                Result<QuestionExplanation> explanationResult = QuestionExplanation.Create(source.Explanation);

                if (explanationResult.IsFailure)
                {
                    return Result.Failure<Question>(explanationResult.Error);
                }

                explanation = explanationResult.Value;
            }

            Result<QuestionNumber> questionNumberResult = QuestionNumber.Create(source.QuestionNumber.Value);

            if (questionNumberResult.IsFailure)
            {
                return Result.Failure<Question>(questionNumberResult.Error);
            }

            return Question.Create(
                questionId,
                contentResult.Value,
                explanation,
                questionNumberResult.Value,
                answers: answers,
                imageUri: null);
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
