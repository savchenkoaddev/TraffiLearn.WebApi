using Riok.Mapperly.Abstractions;
using TraffiLearn.Application.DTO.Questions.Request;
using TraffiLearn.Application.DTO.Questions.Response;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.Services.Mappers
{
    [Mapper]
    public partial class QuestionMapper
    {
        public partial Question ToEntity(QuestionAddRequest request);

        public partial Question ToEntity(QuestionUpdateRequest request);

        public partial QuestionResponse ToResponse(Question question);

        public partial IEnumerable<QuestionResponse> ToResponse(IEnumerable<Question> questions);
    }
}
