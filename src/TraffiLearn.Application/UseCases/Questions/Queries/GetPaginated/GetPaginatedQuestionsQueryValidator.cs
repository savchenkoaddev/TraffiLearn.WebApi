using FluentValidation;

namespace TraffiLearn.Application.UseCases.Questions.Queries.GetPaginated
{
    internal sealed class GetPaginatedQuestionsQueryValidator
        : AbstractValidator<GetPaginatedQuestionsQuery>
    {
        public GetPaginatedQuestionsQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 30);
        }
    }
}
