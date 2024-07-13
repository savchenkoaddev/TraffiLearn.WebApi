using MediatR;
using TraffiLearn.Application.DTO.Questions.Request;

namespace TraffiLearn.Application.Questions.Commands.CreateQuestion
{
    public sealed record CreateQuestionCommand(
        QuestionCreateRequest? RequestObject) : IRequest;
}
