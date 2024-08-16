namespace TraffiLearn.Domain.Primitives
{
    public sealed class AggregateRoot<TId> : Entity<TId>
    {
        public AggregateRoot(TId id)
            : base(id)
        { }
    }
}
