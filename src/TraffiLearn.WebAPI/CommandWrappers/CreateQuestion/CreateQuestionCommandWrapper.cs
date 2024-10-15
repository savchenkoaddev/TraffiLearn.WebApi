using Swashbuckle.AspNetCore.JsonMultipartFormDataSupport.Attributes;
using TraffiLearn.Application.Questions.Commands.Create;

namespace TraffiLearn.WebAPI.CommandWrappers.CreateQuestion
{
    public sealed class CreateQuestionCommandWrapper
    {
        public IFormFile? Image { get; init; }

        [FromJson]
        public required CreateQuestionRequest Request { get; init; }

        public CreateQuestionCommand ToCommand()
        {
            return new CreateQuestionCommand(
                Content: Request.Content,
                Explanation: Request.Explanation,
                QuestionNumber: Request.QuestionNumber,
                TopicIds: Request.TopicIds,
                Answers: Request.Answers,
                Image: Image);
        }
    }
}
