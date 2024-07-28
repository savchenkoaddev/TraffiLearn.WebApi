using MediatR;
using TraffiLearn.Application.DTO.Questions;

namespace TraffiLearn.Application.Queries.Questions.GetById
{
    public sealed record GetQuestionByIdQuery(
        Guid? QuestionId) : IRequest<QuestionResponse>;
}
