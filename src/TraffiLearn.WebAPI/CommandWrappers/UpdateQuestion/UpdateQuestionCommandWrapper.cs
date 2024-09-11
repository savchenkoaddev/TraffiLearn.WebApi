using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;
using TraffiLearn.Application.Questions.Commands.Update;

namespace TraffiLearn.WebAPI.CommandWrappers.UpdateQuestion
{
    public sealed class UpdateQuestionCommandWrapper
    {
        public IFormFile? Image { get; init; }

        [FromJson]
        public UpdateQuestionRequest Request { get; init; }

        public UpdateQuestionCommand ToCommand()
        {
            return new UpdateQuestionCommand(
                QuestionId: Request.QuestionId,
                Content: Request.Content,
                Explanation: Request.Explanation,
                QuestionNumber: Request.QuestionNumber,
                TopicIds: Request.TopicIds,
                Answers: Request.Answers,
                Image: Image,
                RemoveOldImageIfNewImageMissing: Request.RemoveOldImageIfNewImageMissing);
        }
    }
}
