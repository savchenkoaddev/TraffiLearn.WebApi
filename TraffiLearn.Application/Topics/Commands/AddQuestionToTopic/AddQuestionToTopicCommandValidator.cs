using FluentValidation;

namespace TraffiLearn.Application.Topics.Commands.AddQuestionToTopic
{
    public sealed class AddQuestionToTopicCommandValidator : AbstractValidator<AddQuestionToTopicCommand>
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
