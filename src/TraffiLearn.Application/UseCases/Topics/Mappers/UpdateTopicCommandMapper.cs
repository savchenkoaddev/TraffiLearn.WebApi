using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.UseCases.Topics.Commands.Update;
using TraffiLearn.Domain.Topics;
using TraffiLearn.Domain.Topics.TopicNumbers;
using TraffiLearn.Domain.Topics.TopicTitles;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Topics.Mappers
{
    internal sealed class UpdateTopicCommandMapper
        : Mapper<UpdateTopicCommand, Result<Topic>>
    {
        public override Result<Topic> Map(UpdateTopicCommand source)
        {
            Result<TopicNumber> topicNumberResult = TopicNumber.Create(source.TopicNumber);

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
                title: topicTitleResult.Value,
                imageUri: null);
        }
    }
}
