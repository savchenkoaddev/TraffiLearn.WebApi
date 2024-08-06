using FluentValidation;

namespace TraffiLearn.Application.Queries.Users.GetUserDislikedQuestions
{
    internal sealed class GetUserDislikedQuestionsQueryValidator
        : AbstractValidator<GetUserDislikedQuestionsQuery>
    {
        public GetUserDislikedQuestionsQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
