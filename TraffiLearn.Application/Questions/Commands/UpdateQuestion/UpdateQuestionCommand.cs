using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.DTO.Questions.Request;

namespace TraffiLearn.Application.Questions.Commands.UpdateQuestion
{
    public sealed record UpdateQuestionCommand(
        Guid QuestionId,
        QuestionUpdateRequest RequestObject,
        IFormFile? Image) : IRequest;
}
