using FluentValidation;

namespace TraffiLearn.Application.Topics.Queries.GetById
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
