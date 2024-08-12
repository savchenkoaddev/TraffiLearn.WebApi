using FluentValidation;

namespace TraffiLearn.Application.Queries.Users.GetUserLikedQuestions
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
