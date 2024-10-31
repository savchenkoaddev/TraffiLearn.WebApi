namespace TraffiLearn.SharedKernel.Primitives
{
    public interface IHasDomainEvents
    {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }

        void ClearDomainEvents();
    }
}
