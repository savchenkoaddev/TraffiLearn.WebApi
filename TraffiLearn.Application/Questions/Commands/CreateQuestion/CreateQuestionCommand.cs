using MediatR;
using Microsoft.AspNetCore.Http;
using TraffiLearn.Application.DTO.Questions.Request;

namespace TraffiLearn.Application.Questions.Commands.CreateQuestion
{
    public sealed record CreateQuestionCommand(
        QuestionCreateRequest RequestObject,
        IFormFile? Image) : IRequest;
}
