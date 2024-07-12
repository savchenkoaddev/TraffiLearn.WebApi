using FluentValidation;

namespace TraffiLearn.Application.Topics.Commands.CreateTopic
{
    public sealed class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
    {
        public CreateTopicCommandValidator()
        {
            RuleFor(x => x.RequestObject.Number)
                .NotEmpty()
                .GreaterThanOrEqualTo(1);

            RuleFor(x => x.RequestObject.Title)
                .NotEmpty()
                .MaximumLength(300);
        }
    }
}
