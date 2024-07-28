using FluentValidation;

namespace TraffiLearn.Application.Commands.Topics.Create
{
    public sealed class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
    {
        public CreateTopicCommandValidator()
        {
            RuleFor(x => x.TopicNumber)
                .GreaterThan(0);

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(300);
        }
    }
}
