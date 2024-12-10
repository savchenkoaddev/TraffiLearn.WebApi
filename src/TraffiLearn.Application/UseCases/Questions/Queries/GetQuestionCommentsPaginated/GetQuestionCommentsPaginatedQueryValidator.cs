using FluentValidation;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetQuestionCommentsPaginated
{
    internal sealed class GetQuestionCommentsPaginatedQueryValidator
        : AbstractValidator<GetQuestionCommentsPaginatedQuery>
    {
        public GetQuestionCommentsPaginatedQueryValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();

            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 50);
        }
    }
}
