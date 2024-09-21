using FluentValidation;

namespace TraffiLearn.Application.Questions.Queries.GetAll
{
    internal sealed class GetAllQuestionsQueryValidator 
        : AbstractValidator<GetAllQuestionsQuery>
    {
        public GetAllQuestionsQueryValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 30);
        }
    }
}
