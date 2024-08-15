using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.DTO.Topics;
using TraffiLearn.Domain.Entities;

namespace TraffiLearn.Application.Mappers.Topics
{
    internal sealed class TopicToTopicWithQuestionsResponseMapper : Mapper<Topic, TopicWithQuestionsResponse>
    {
        public override TopicWithQuestionsResponse Map(Topic source)
        {
            return new TopicWithQuestionsResponse(
                TopicId: source.Id.Value,
                TopicNumber: source.Number.Value,
                Title: source.Title.Value,
                Questions: source.Questions.ToList());
        }
    }
}
