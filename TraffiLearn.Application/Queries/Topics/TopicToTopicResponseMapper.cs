using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.Queries.Topics
{
    internal sealed class TopicToTopicResponseMapper : Mapper<Topic, TopicResponse>
    {
        public override TopicResponse Map(Topic source)
        {
            return new TopicResponse(
                Id: source.Id,
                TopicNumber: source.Number.Value,
                Title: source.Title.Value);
        }
    }
}
