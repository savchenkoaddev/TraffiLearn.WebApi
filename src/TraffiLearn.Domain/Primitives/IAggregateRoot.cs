﻿namespace TraffiLearn.Domain.Primitives
{
    public interface IAggregateRoot
    {
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }

        void ClearDomainEvents();
    }
}