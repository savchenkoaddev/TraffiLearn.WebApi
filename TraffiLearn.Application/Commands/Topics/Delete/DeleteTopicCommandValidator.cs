using FluentValidation;

namespace TraffiLearn.Application.Commands.Topics.Delete
{
    public sealed class DeleteTopicCommandValidator : AbstractValidator<DeleteTopicCommand>
    {
        public DeleteTopicCommandValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty();
        }
    }
}
