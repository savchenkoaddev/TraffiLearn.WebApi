using MediatR;
using TraffiLearn.Application.DTO.Questions;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Queries.Users.GetMarkedQuestions
{
    public sealed record GetMarkedQuestionsQuery 
        : IRequest<Result<IEnumerable<QuestionResponse>>>;
}
