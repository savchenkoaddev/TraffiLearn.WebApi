using Riok.Mapperly.Abstractions;
using TraffiLearn.Application.DTO.Topics.Request;
using TraffiLearn.Application.DTO.Topics.Response;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.Topics
{
    [Mapper]
    public partial class TopicMapper
    {
        public partial Topic ToEntity(TopicRequest request);

        public partial TopicResponse ToResponse(Topic topic);

        public partial IEnumerable<TopicResponse> ToResponse(IEnumerable<Topic> topics);
    }
}
