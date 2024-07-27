using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics.Response;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.Topics.Mappers
{
    internal sealed class TopicToTopicResponseMapper : Mapper<Topic, TopicResponse>
    {
        public override TopicResponse Map(Topic source)
        {
            return new TopicResponse(
                Id: source.Id,
                Number: source.Number,
                Title: source.Title);
        }
    }
}
