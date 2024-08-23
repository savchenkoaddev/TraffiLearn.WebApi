using FluentValidation;

namespace TraffiLearn.Application.Questions.Commands.Update
{
    internal sealed class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
    {
        public UpdateQuestionCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(2000);

            RuleFor(x => x.Explanation)
                .NotEmpty()
                .MaximumLength(2000);

            RuleFor(x => x.QuestionNumber)
                .GreaterThan(0)
                .NotEmpty();

            RuleFor(x => x.Answers)
                .NotEmpty();

            RuleForEach(x => x.Answers)
                .NotEmpty()
                .ChildRules(x =>
                {
                    x.RuleFor(x => x.Text)
                        .NotEmpty()
                        .MaximumLength(300);

                    x.RuleFor(x => x.IsCorrect)
                        .NotEmpty();
                })
                .When(x => x.Answers is not null);

            RuleFor(x => x.TopicIds)
                .NotEmpty();

            RuleForEach(x => x.TopicIds)
                .NotEmpty()
                .When(x => x.TopicIds is not null);
        }
    }
}
