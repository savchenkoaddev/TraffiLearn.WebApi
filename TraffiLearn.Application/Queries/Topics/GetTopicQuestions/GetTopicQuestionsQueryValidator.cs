using FluentValidation;

namespace TraffiLearn.Application.Queries.Topics.GetTopicQuestions
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
