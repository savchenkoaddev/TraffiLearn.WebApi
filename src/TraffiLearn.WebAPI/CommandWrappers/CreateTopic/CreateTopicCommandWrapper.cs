using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;
using TraffiLearn.Application.UseCases.Topics.Commands.Create;

namespace TraffiLearn.WebAPI.CommandWrappers.CreateTopic
{
    public sealed class CreateTopicCommandWrapper
    {
        public IFormFile? Image { get; init; }

        [FromJson]
        public required CreateTopicRequest Request { get; init; }

        public CreateTopicCommand ToCommand()
        {
            return new CreateTopicCommand(
                TopicNumber: Request.TopicNumber,
                Title: Request.Title,
                Image: Image);
        }
    }
}
