using MediatR;
using TraffiLearn.Application.Questions.DTO;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Questions.Queries.GetById
{
    public sealed record GetQuestionByIdQuery(
        Guid? QuestionId) : IRequest<Result<QuestionResponse>>;
}
