using FluentValidation;

namespace TraffiLearn.Application.UseCases.Topics.Commands.Delete
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
