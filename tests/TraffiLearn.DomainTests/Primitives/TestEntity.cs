using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.DomainTests.Primitives
{
    internal sealed class TestEntity : Entity<Guid>
    {
        public TestEntity(Guid id)
            : base(id)
        { }
    }
}
