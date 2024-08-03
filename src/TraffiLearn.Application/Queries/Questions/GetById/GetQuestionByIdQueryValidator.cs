using FluentValidation;

namespace TraffiLearn.Application.Queries.Questions.GetById
{
    internal sealed class GetQuestionByIdQueryValidator : AbstractValidator<GetQuestionByIdQuery>
    {
        public GetQuestionByIdQueryValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
