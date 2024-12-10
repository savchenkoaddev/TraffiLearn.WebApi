using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;
using TraffiLearn.Application.UseCases.Topics.Commands.Update;

namespace TraffiLearn.WebAPI.CommandWrappers.UpdateTopic
{
    public sealed class UpdateTopicCommandWrapper
    {
        public IFormFile? Image { get; init; }

        [FromJson]
        public required UpdateTopicRequest Request { get; init; }

        public UpdateTopicCommand ToCommand()
        {
            return new UpdateTopicCommand(
                TopicId: Request.TopicId,
                TopicNumber: Request.TopicNumber,
                Title: Request.Title,
                Image: Image,
                RemoveOldImageIfNewMissing: Request.RemoveOldImageIfNewMissing ?? true);
        }
    }
}
