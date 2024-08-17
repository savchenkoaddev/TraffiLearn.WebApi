using FluentValidation;

namespace TraffiLearn.Application.Topics.Commands.Delete
{
    internal sealed class DeleteTopicCommandValidator : AbstractValidator<DeleteTopicCommand>
    {
        public DeleteTopicCommandValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty();
        }
    }
}
