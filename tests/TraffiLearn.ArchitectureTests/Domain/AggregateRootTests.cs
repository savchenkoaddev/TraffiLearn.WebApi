using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.ArchitectureTests.Domain
{
    public sealed class AggregateRootTests : BaseTest
    {
        [Fact]
        public void AggregateRootBaseClass_ShouldInheritFromIHasDomainEvents()
        {
            Type aggregateRootType = typeof(AggregateRoot<>);

            typeof(IHasDomainEvents).IsAssignableFrom(aggregateRootType)
                .Should().BeTrue("AggregateRoot should implement IHasDomainEvents.");
        }

        [Fact]
        public void AggregateRootBaseClass_ShouldHaveProtectedRaiseDomainEventMethod()
        {
            Type aggregateRootType = typeof(AggregateRoot<>);

            MethodInfo? methodInfo = aggregateRootType
                .GetMethod("RaiseDomainEvent", BindingFlags.NonPublic | BindingFlags.Instance);

            // Check if the method exists and is protected
            methodInfo.Should().NotBeNull("AggregateRoot should have a protected method named RaiseDomainEvent.");
            methodInfo.IsFamily.Should().BeTrue("RaiseDomainEvent should be a protected method.");
        }

        [Fact]
        public void AggregateRootBaseClass_ShouldNotHavePublicFields()
        {
            Type aggregateRootType = typeof(AggregateRoot<>);

            var fields = aggregateRootType.GetFields(BindingFlags.Public | BindingFlags.Instance);

            fields.Should().BeEmpty("AggregateRoot should not have public fields.");
        }
    }
}
