using MediatR;
using TraffiLearn.Application.DTO.Questions.Response;

namespace TraffiLearn.Application.Questions.Queries.GetQuestionById
{
    public sealed record GetQuestionByIdQuery(
        Guid QuestionId) : IRequest<QuestionResponse>;
}
