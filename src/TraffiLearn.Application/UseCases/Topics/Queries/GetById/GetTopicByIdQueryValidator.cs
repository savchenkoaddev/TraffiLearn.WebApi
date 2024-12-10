using FluentValidation;

namespace TraffiLearn.Application.UseCases.Topics.Queries.GetById
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
