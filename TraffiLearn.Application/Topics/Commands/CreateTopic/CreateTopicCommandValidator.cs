using FluentValidation;

namespace TraffiLearn.Application.Topics.Commands.CreateTopic
{
    public sealed class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
    {
        public CreateTopicCommandValidator()
        {
            RuleFor(x => x.RequestObject)
                .NotEmpty();

            RuleFor(x => x.RequestObject.Number)
                .NotEmpty()
                .GreaterThanOrEqualTo(1)
                .When(x => x.RequestObject is not null);

            RuleFor(x => x.RequestObject.Title)
                .NotEmpty()
                .MaximumLength(300)
                .When(x => x.RequestObject is not null);
        }
    }
}
