using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Commands.Topics.Update;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Mappers.Topics
{
    internal sealed class UpdateTopicCommandMapper
        : Mapper<UpdateTopicCommand, Result<Domain.Aggregates.Topics.Topic>>
    {
        public override Result<Domain.Aggregates.Topics.Topic> Map(UpdateTopicCommand source)
        {
            Result<TopicNumber> topicNumberResult = TopicNumber.Create(source.TopicNumber.Value);

            if (topicNumberResult.IsFailure)
            {
                return Result.Failure<Domain.Aggregates.Topics.Topic>(topicNumberResult.Error);
            }

            Result<TopicTitle> topicTitleResult = TopicTitle.Create(source.Title);

            if (topicTitleResult.IsFailure)
            {
                return Result.Failure<Domain.Aggregates.Topics.Topic>(topicTitleResult.Error);
            }

            TopicId topicId = new(Guid.NewGuid());

            return TopicId.Create(
                topicId: topicId,
                number: topicNumberResult.Value,
                title: topicTitleResult.Value);
        }
    }
}
