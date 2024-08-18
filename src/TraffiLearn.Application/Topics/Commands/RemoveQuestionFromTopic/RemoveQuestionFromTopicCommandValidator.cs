using FluentValidation;

namespace TraffiLearn.Application.Topics.Commands.RemoveQuestionFromTopic
{
    internal sealed class RemoveQuestionFromTopicCommandValidator : AbstractValidator<RemoveQuestionFromTopicCommand>
    {
        public RemoveQuestionFromTopicCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();

            RuleFor(x => x.TopicId)
                .NotEmpty();
        }
    }
}
