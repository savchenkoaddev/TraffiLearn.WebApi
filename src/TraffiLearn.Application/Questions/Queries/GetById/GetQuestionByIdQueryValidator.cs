using FluentValidation;

namespace TraffiLearn.Application.Questions.Queries.GetById
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
