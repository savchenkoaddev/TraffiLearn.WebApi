using TraffiLearn.Domain.Aggregates.Topics;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Aggregates.Questions.DomainEvents
{
    public sealed record TopicAddedToQuestionDomainEvent(
        Guid Id,
        Topic Topic,
        Question Question) : DomainEvent(Id);
}
