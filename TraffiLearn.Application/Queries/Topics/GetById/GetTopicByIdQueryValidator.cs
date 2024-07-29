using FluentValidation;

namespace TraffiLearn.Application.Queries.Topics.GetById
{
    internal sealed class GetTopicByIdQueryValidator : AbstractValidator<GetTopicByIdQuery>
    {
        public GetTopicByIdQueryValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty();
        }
    }
}
