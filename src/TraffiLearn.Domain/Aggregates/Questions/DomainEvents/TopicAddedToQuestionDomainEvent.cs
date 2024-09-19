using TraffiLearn.Domain.Aggregates.Questions.ValueObjects;
using TraffiLearn.Domain.Aggregates.Topics.ValueObjects;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Aggregates.Questions.DomainEvents
{
    public sealed record TopicAddedToQuestionDomainEvent(
        Guid Id,
        TopicId TopicId,
        QuestionId QuestionId) : DomainEvent(Id);
}
