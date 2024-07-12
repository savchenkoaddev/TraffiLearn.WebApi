using FluentValidation;

namespace TraffiLearn.Application.Topics.Commands.DeleteTopic
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
