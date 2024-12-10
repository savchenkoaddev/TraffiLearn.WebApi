using FluentValidation;

namespace TraffiLearn.Application.UseCases.Users.Queries.GetUserComments
{
    internal sealed class GetUserCommentsQueryValidator
        : AbstractValidator<GetUserCommentsQuery>
    {
        public GetUserCommentsQueryValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty();
        }
    }
}
