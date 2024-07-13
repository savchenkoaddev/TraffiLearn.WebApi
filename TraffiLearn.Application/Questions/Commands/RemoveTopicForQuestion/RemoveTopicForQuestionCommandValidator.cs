using FluentValidation;

namespace TraffiLearn.Application.Questions.Commands.RemoveTopicForQuestion
{
    public sealed class RemoveTopicForQuestionCommandValidator : AbstractValidator<RemoveTopicForQuestionCommand>
    {
        public RemoveTopicForQuestionCommandValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty();

            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
