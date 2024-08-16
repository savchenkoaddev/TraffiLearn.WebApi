using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Commands.Topics.Create;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Mapper.Topics
{
    internal sealed class CreateTopicCommandMapper
        : Mapper<CreateTopicCommand, Result<Domain.Aggregates.Topics.Topic>>
    {
        public override Result<Domain.Aggregates.Topics.Topic> Map(CreateTopicCommand source)
        {
            Domain.Aggregates.Topics.ValueObjects.TopicId topicId = new(Guid.NewGuid());

            Result<TopicNumber> numberCreateResult = TopicNumber.Create(source.TopicNumber.Value);

            if (numberCreateResult.IsFailure)
            {
                return Result.Failure<Domain.Aggregates.Topics.Topic>(numberCreateResult.Error);
            }

            Result<TopicTitle> titleCreateResult = TopicTitle.Create(source.Title);

            if (titleCreateResult.IsFailure)
            {
                return Result.Failure<Domain.Aggregates.Topics.Topic>(titleCreateResult.Error);
            }

            return TopicId.Create(
                topicId: topicId,
                number: numberCreateResult.Value,
                title: titleCreateResult.Value);
        }
    }
}
