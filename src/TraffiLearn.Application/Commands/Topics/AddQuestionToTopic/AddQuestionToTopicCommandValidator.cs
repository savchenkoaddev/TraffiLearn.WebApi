using FluentValidation;

namespace TraffiLearn.Application.Commands.Topics.AddQuestionToTopic
{
    internal sealed class AddQuestionToTopicCommandValidator : AbstractValidator<AddQuestionToTopicCommand>
    {
        public AddQuestionToTopicCommandValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty();

            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
