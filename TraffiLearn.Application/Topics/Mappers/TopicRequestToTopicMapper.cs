using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics.Request;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.Topics.Mappers
{
    internal sealed class TopicRequestToTopicMapper : Mapper<TopicRequest, Topic>
    {
        public override Topic Map(TopicRequest source)
        {
            return Topic.Create(
                id: Guid.NewGuid(),
                number: source.Number,
                title: source.Title);
        }
    }
}
