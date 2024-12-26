using MediatR;
using TraffiLearn.Application.UseCases.Questions.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetById
{
    public sealed record GetQuestionByIdQuery(
        Guid QuestionId) : IRequest<Result<QuestionResponse>>;
}
