namespace TraffiLearn.Domain.Primitives
{
    public interface IEntity
    {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }
    }
}
