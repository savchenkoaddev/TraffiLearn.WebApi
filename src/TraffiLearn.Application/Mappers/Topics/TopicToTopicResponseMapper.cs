using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Domain.Aggregates.Topics;

namespace TraffiLearn.Application.Mapper.Topics
{
    internal sealed class TopicToTopicResponseMapper : Mapper<Topic, TopicResponse>
    {
        public override TopicResponse Map(Topic source)
        {
            return new TopicResponse(
                TopicId: source.Id.Value,
                TopicNumber: source.Number.Value,
                Title: source.Title.Value);
        }
    }
}
