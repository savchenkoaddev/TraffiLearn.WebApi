using FluentValidation;

namespace TraffiLearn.Application.Queries.Users.GetUserComments
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
