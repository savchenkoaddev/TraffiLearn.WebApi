using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Answers.Response;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Application.DTO.QuestionTitleDetails.Response;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.ValueObjects;

namespace TraffiLearn.Application.Questions.Mappers
{
    internal sealed class QuestionToQuestionResponseMapper : Mapper<Question, QuestionResponse>
    {
        private readonly Mapper<Answer, AnswerResponse> _answersMapper;
        private readonly Mapper<QuestionTitleDetails, QuestionTitleDetailsResponse> _questionTitleDetailsMapper;

        public QuestionToQuestionResponseMapper(
            Mapper<Answer, AnswerResponse> answersMapper,
            Mapper<QuestionTitleDetails, QuestionTitleDetailsResponse> questionTitleDetailsMapper)
        {
            _answersMapper = answersMapper;
            _questionTitleDetailsMapper = questionTitleDetailsMapper;
        }

        public override QuestionResponse Map(Question source)
        {
            var questionTitleDetailsResponse = _questionTitleDetailsMapper.Map(source.QuestionTitleDetails);
            var answersResponses = _answersMapper.Map(source.Answers).ToList();

            return new QuestionResponse(
                Id: source.Id,
                Content: source.Content,
                Explanation: source.Explanation,
                ImageUri: source.ImageUri,
                LikesCount: source.LikesCount,
                DislikesCount: source.DislikesCount,
                QuestionTitleDetails: questionTitleDetailsResponse,
                Answers: answersResponses);
        }
    }
}
