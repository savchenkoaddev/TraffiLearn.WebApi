using FluentValidation;

namespace TraffiLearn.Application.Queries.Topics.GetQuestionsForTopic
{
    internal sealed class GetQuestionsForTopicQueryValidator : AbstractValidator<GetQuestionsForTopicQuery>
    {
        public GetQuestionsForTopicQueryValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty();
        }
    }
}
