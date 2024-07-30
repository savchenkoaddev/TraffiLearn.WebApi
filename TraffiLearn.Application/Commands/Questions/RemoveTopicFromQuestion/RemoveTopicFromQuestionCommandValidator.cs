using FluentValidation;

namespace TraffiLearn.Application.Commands.Questions.RemoveTopicFromQuestion
{
    internal sealed class RemoveTopicFromQuestionCommandValidator : AbstractValidator<RemoveTopicFromQuestionCommand>
    {
        public RemoveTopicFromQuestionCommandValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty();

            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
