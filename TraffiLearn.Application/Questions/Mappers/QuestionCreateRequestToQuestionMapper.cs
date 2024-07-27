using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Answers.Request;
using TraffiLearn.Application.DTO.Questions.Request;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.Questions.Mappers
{
    internal sealed class QuestionCreateRequestToQuestionMapper : Mapper<QuestionCreateRequest, Question>
    {
        private readonly Mapper<AnswerRequest, Answer> _answerMapper;

        public QuestionCreateRequestToQuestionMapper(Mapper<AnswerRequest, Answer> answerMapper)
        {
            _answerMapper = answerMapper;
        }

        public override Question Map(QuestionCreateRequest source)
        {
            var questionTitleDetails = QuestionTitleDetails.Create(
                ticketNumber: source.TitleDetails.TicketNumber,
                questionNumber: source.TitleDetails.QuestionNumber);

            return Question.Create(
                id: Guid.NewGuid(),
                content: source.Content,
                explanation: source.Explanation,
                questionTitleDetails: questionTitleDetails,
                answers: _answerMapper.Map(source.Answers).ToList(),
                imageUri: null);
        }
    }
}
