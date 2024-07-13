using FluentValidation;

namespace TraffiLearn.Application.Questions.Commands.AddTopicToQuestion
{
    public sealed class AddTopicToQuestionCommandValidator : AbstractValidator<AddTopicToQuestionCommand>
    {
        public AddTopicToQuestionCommandValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty();

            RuleFor(x => x.QuestionId)
                .NotEmpty();
        }
    }
}
