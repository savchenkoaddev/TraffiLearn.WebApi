using FluentValidation;

namespace TraffiLearn.Application.Commands.Topics.Update
{
    internal sealed class UpdateTopicCommandValidator : AbstractValidator<UpdateTopicCommand>
    {
        public UpdateTopicCommandValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty();

            RuleFor(x => x.TopicNumber)
                .GreaterThan(0);

            RuleFor(x => x.Title)
                .NotEmpty()
                .MaximumLength(300);
        }
    }
}
