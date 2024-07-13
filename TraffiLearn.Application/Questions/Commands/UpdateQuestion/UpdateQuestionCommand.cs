using MediatR;
using TraffiLearn.Application.DTO.Questions.Request;

namespace TraffiLearn.Application.Questions.Commands.UpdateQuestion
{
    public sealed record UpdateQuestionCommand(
        Guid? QuestionId,
        QuestionUpdateRequest? RequestObject) : IRequest;
}
