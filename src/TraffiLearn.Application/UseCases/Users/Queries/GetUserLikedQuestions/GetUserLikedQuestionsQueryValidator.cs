using FluentValidation;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetUserLikedQuestions
{
    internal sealed class GetUserLikedQuestionsQueryValidator
        : AbstractValidator<GetUserLikedQuestionsQuery>
    {
        public GetUserLikedQuestionsQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
