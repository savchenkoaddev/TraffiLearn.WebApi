using FluentValidation;
using MediatR;

namespace TraffiLearn.Application.Commands.Questions.Update
{
    public sealed class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
    {
        public UpdateQuestionCommandValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty();

            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(2000);

            RuleFor(x => x.Explanation)
                .NotEmpty()
                .MaximumLength(2000);

            RuleFor(x => x.TicketNumber)
                .GreaterThan(0);

            RuleFor(x => x.QuestionNumber)
                .GreaterThan(0);

            RuleFor(x => x.Answers)
                .NotEmpty();

            RuleFor(x => x.TopicsIds)
                .NotEmpty();

            RuleForEach(x => x.TopicsIds)
                .NotEmpty();
        }
    }
}
