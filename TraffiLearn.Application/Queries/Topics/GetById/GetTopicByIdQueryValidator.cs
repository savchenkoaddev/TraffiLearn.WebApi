using FluentValidation;

namespace TraffiLearn.Application.Queries.Topics.GetById
{
    public sealed class GetTopicByIdQueryValidator : AbstractValidator<GetTopicByIdQuery>
    {
        public GetTopicByIdQueryValidator()
        {
            RuleFor(x => x.TopicId)
                .NotEmpty();
        }
    }
}
