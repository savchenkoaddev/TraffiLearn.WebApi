using FluentValidation;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetUserDislikedQuestions
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
