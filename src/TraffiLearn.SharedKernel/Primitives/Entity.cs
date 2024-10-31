namespace TraffiLearn.SharedKernel.Primitives
{
    public abstract class Entity<TId> : IEquatable<Entity<TId>>
         where TId : notnull
    {
        protected Entity(TId id)
        {
            Id = id;
        }

        public TId Id { get; private init; }

        public static bool operator ==(Entity<TId>? first, Entity<TId>? second)
        {
            if (first is null && second is null)
            {
                return true;
            }

            if (first is null || second is null)
            {
                return false;
            }

            return first.Equals(second);
        }

        public static bool operator !=(Entity<TId>? first, Entity<TId>? second)
        {
            return !(first == second);
        }

        public override bool Equals(object? obj)
        {
            if (obj is Entity<TId> entity)
            {
                return Equals(entity);
            }

            return false;
        }

        public bool Equals(Entity<TId>? other)
        {
            if (other is null || other.GetType() != GetType())
            {
                return false;
            }

            return EqualityComparer<TId>.Default.Equals(Id, other.Id);
        }

        public override int GetHashCode()
        {
            return EqualityComparer<TId>.Default.GetHashCode(Id);
        }
    }
}
