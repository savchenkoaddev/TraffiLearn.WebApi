using FluentValidation;

namespace TraffiLearn.Application.Commands.Topics.RemoveQuestionForTopic
{
    public sealed class RemoveQuestionForTopicCommandValidator : AbstractValidator<RemoveQuestionForTopicCommand>
    {
        public RemoveQuestionForTopicCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();

            RuleFor(x => x.TopicId)
                .NotEmpty();
        }
    }
}
