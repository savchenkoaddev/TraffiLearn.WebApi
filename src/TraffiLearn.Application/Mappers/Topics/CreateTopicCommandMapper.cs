using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Commands.Topics.Create;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Topics;

namespace TraffiLearn.Application.Mapper.Topics
{
    internal sealed class CreateTopicCommandMapper
        : Mapper<CreateTopicCommand, Result<Topic>>
    {
        public override Result<Topic> Map(CreateTopicCommand source)
        {
            var topicId = Guid.NewGuid();

            Result<TopicNumber> numberCreateResult = TopicNumber.Create(source.TopicNumber.Value);

            if (numberCreateResult.IsFailure)
            {
                return Result.Failure<Topic>(numberCreateResult.Error);
            }

            Result<TopicTitle> titleCreateResult = TopicTitle.Create(source.Title);

            if (titleCreateResult.IsFailure)
            {
                return Result.Failure<Topic>(titleCreateResult.Error);
            }

            return Topic.Create(
                id: topicId,
                number: numberCreateResult.Value,
                title: titleCreateResult.Value);
        }
    }
}
