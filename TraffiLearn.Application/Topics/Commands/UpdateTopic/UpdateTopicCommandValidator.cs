using FluentValidation;

namespace TraffiLearn.Application.Topics.Commands.UpdateTopic
{
    public sealed class UpdateTopicCommandValidator : AbstractValidator<UpdateTopicCommand>
    {
        public UpdateTopicCommandValidator()
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
