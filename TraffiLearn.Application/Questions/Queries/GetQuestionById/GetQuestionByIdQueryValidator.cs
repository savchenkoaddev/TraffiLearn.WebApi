using FluentValidation;

namespace TraffiLearn.Application.Questions.Queries.GetQuestionById
{
    public sealed class GetQuestionByIdQueryValidator : AbstractValidator<GetQuestionByIdQuery>
    {
        public GetQuestionByIdQueryValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
