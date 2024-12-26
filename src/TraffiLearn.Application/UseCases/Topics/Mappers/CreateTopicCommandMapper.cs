using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Topics.Commands.Create;
using TraffiLearn.Domain.Topics;
using TraffiLearn.Domain.Topics.TopicNumbers;
using TraffiLearn.Domain.Topics.TopicTitles;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Topics.Mappers
{
    internal sealed class CreateTopicCommandMapper
        : Mapper<CreateTopicCommand, Result<Topic>>
    {
        public override Result<Topic> Map(CreateTopicCommand source)
        {
            TopicId topicId = new(Guid.NewGuid());

            Result<TopicNumber> numberCreateResult = TopicNumber.Create(source.TopicNumber);

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
                topicId: topicId,
                number: numberCreateResult.Value,
                title: titleCreateResult.Value);
        }
    }
}
