using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.DomainTests.Primitives
{
    internal sealed class AnotherTestEntity : Entity<Guid>
    {
        public AnotherTestEntity(Guid id)
            : base(id)
        { }
    }
}
