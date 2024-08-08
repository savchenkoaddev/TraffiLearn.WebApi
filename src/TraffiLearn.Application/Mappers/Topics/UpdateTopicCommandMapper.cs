using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Commands.Topics.Update;
using TraffiLearn.Domain.Entities;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Topics;

namespace TraffiLearn.Application.Mapper.Topics
{
    internal sealed class UpdateTopicCommandMapper
        : Mapper<UpdateTopicCommand, Result<Topic>>
    {
        public override Result<Topic> Map(UpdateTopicCommand source)
        {
            Result<TopicNumber> topicNumberResult = TopicNumber.Create(source.TopicNumber.Value);

            if (topicNumberResult.IsFailure)
            {
                return Result.Failure<Topic>(topicNumberResult.Error);
            }

            Result<TopicTitle> topicTitleResult = TopicTitle.Create(source.Title);

            if (topicTitleResult.IsFailure)
            {
                return Result.Failure<Topic>(topicTitleResult.Error);
            }

            TopicId topicId = new(Guid.NewGuid());

            return Topic.Create(
                topicId: topicId,
                number: topicNumberResult.Value,
                title: topicTitleResult.Value);
        }
    }
}
