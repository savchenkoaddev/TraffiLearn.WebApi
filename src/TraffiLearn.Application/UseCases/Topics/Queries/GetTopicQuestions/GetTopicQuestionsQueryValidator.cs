using FluentValidation;

namespace TraffiLearn.Application.UseCases.Topics.Queries.GetTopicQuestions
{
    internal sealed class GetTopicQuestionsQueryValidator : AbstractValidator<GetTopicQuestionsQuery>
    {
        public GetTopicQuestionsQueryValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty();
        }
    }
}
