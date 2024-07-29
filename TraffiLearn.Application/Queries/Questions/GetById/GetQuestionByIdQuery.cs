using MediatR;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Questions.GetById
{
    public sealed record GetQuestionByIdQuery(
        Guid? QuestionId) : IRequest<Result<QuestionResponse>>;
}
