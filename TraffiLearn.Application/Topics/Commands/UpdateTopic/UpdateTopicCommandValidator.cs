using FluentValidation;

namespace TraffiLearn.Application.Topics.Commands.UpdateTopic
{
    public sealed class UpdateTopicCommandValidator : AbstractValidator<UpdateTopicCommand>
    {
        public UpdateTopicCommandValidator()
        {
            RuleFor(x => x.RequestObject)
                .NotEmpty();

            RuleFor(x => x.TopicId)
                .NotEmpty()
                .When(x => x.RequestObject is not null);

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
