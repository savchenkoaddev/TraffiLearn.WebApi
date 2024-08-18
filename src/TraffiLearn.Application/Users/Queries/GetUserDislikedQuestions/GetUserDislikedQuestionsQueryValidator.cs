using FluentValidation;

namespace TraffiLearn.Application.Users.Queries.GetUserDislikedQuestions
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
