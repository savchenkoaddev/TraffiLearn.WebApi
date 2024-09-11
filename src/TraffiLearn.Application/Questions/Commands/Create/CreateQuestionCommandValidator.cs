using FluentValidation;
using TraffiLearn.Application.Validators;
using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;

namespace TraffiLearn.Application.Questions.Commands.Create
{
    internal sealed class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
    {
        public CreateQuestionCommandValidator()
        {
            RuleFor(x => x.Explanation)
                .MaximumLength(QuestionExplanation.MaxLength)
                .When(x => x.Explanation is not null);

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(QuestionContent.MaxLength);

            RuleFor(x => x.QuestionNumber)
                .GreaterThan(0)
                .NotEmpty();

            RuleFor(x => x.TopicIds)
                .NotEmpty();

            RuleForEach(x => x.TopicIds)
                .NotEmpty()
                .When(x => x.TopicIds is not null);

            RuleFor(x => x.Answers)
                .NotEmpty();

            RuleForEach(x => x.Answers)
                .NotEmpty()
                .ChildRules(x =>
                {
                    x.RuleFor(x => x.Text)
                        .NotEmpty()
                        .MaximumLength(Answer.MaxTextLength);

                    x.RuleFor(x => x.IsCorrect)
                        .NotEmpty();
                })
                .When(x => x.Answers is not null);

            RuleFor(x => x.Image)
               .SetValidator(new ImageValidator())
               .When(x => x.Image is not null);
        }
    }
}
