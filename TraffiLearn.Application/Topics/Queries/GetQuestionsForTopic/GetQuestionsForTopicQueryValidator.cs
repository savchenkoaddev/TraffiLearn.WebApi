using FluentValidation;

namespace TraffiLearn.Application.Topics.Queries.GetQuestionsForTopic
{
    public sealed class GetQuestionsForTopicQueryValidator : AbstractValidator<GetQuestionsForTopicQuery>
    {
        public GetQuestionsForTopicQueryValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty();
        }
    }
}
