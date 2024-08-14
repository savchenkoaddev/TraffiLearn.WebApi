using FluentAssertions;
using System.Reflection;

namespace TraffiLearn.DomainTests.Primitives
{
    public sealed class EntityTests
    {
        [Fact]
        public void Entities_WithSameId_ShouldBeEqual()
        {
            var id = Guid.NewGuid();

            var entity1 = new TestEntity(id);
            var entity2 = new TestEntity(id);

            entity1.Should().Be(entity2);
        }

        [Fact]
        public void Entities_WithDifferentTypes_ShouldNotBeEqual()
        {
            var id = Guid.NewGuid();

            var testEntity = new TestEntity(id);
            var anotherTestEntity = new AnotherTestEntity(id);

            testEntity.Should().NotBe(anotherTestEntity);
        }

        [Fact]
        public void Entity_ShouldNotBeEqualToNull()
        {
            var id = Guid.NewGuid();
            var entity = new TestEntity(id);

            entity.Should().NotBeNull();
            entity.Equals(null).Should().BeFalse();
        }

        [Fact]
        public void Entity_WithSameId_ShouldBeEqualInCollection()
        {
            var id = Guid.NewGuid();

            var entity1 = new TestEntity(id);
            var entity2 = new TestEntity(id);

            var set = new HashSet<TestEntity> { entity1 };

            set.Contains(entity2).Should().BeTrue();
        }

        [Fact]
        public void Entity_ShouldBeEqualUsingOperator()
        {
            var id = Guid.NewGuid();
            var entity1 = new TestEntity(id);
            var entity2 = new TestEntity(id);

            (entity1 == entity2).Should().BeTrue();
            (entity1 != entity2).Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_ForSameId_ShouldBeConsistent()
        {
            var id = Guid.NewGuid();

            var entity1 = new TestEntity(id);
            var entity2 = new TestEntity(id);

            var hashCode1 = entity1.GetHashCode();
            var hashCode2 = entity2.GetHashCode();

            hashCode1.Should().Be(hashCode2);
        }

        [Fact]
        public void GetHashCode_ForDifferentIds_ShouldBeDifferent()
        {
            var entity1 = new TestEntity(Guid.NewGuid());
            var entity2 = new TestEntity(Guid.NewGuid());

            var hashCode1 = entity1.GetHashCode();
            var hashCode2 = entity2.GetHashCode();

            hashCode1.Should().NotBe(hashCode2);
        }

        [Fact]
        public void Entity_ShouldHaveImmutableId()
        {
            var propertyInfo = typeof(TestEntity).GetProperty(nameof(TestEntity.Id), BindingFlags.Public | BindingFlags.Instance);

            propertyInfo?.CanWrite.Should().BeFalse();
        }
    }
}
