using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;
using TraffiLearn.Application.UseCases.Questions.Commands.Update;

namespace TraffiLearn.WebAPI.CommandWrappers.UpdateQuestion
{
    public sealed class UpdateQuestionCommandWrapper
    {
        public IFormFile? Image { get; init; }

        [FromJson]
        public required UpdateQuestionRequest Request { get; init; }

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
                RemoveOldImageIfNewMissing: Request.RemoveOldImageIfNewMissing ?? true);
        }
    }
}
