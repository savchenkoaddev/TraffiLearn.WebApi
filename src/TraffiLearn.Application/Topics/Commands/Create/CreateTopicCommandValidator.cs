using FluentValidation;
using TraffiLearn.Application.Validators;

namespace TraffiLearn.Application.Topics.Commands.Create
{
    internal sealed class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
    {
        public CreateTopicCommandValidator()
        {
            RuleFor(x => x.TopicNumber)
                .GreaterThan(0)
                .NotEmpty();

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(300);

            RuleFor(x => x.Image)
               .SetValidator(new ImageValidator())
               .When(x => x.Image is not null);
        }
    }
}
