using FluentAssertions;
using MediatR;
using NetArchTest.Rules;
using System.Reflection;
using TraffiLearn.SharedKernel.Primitives;

namespace TraffiLearn.ArchitectureTests.Domain
{
    public sealed class DomainEventsTests : BaseTest
    {
        [Fact]
        public void DomainEventBaseClass_ShouldBeAbstract()
        {
            Type domainEventType = typeof(DomainEvent);

            domainEventType.IsAbstract.Should().BeTrue();
        }

        [Fact]
        public void DomainEventBaseClass_ShouldContainIdProperty()
        {
            Type domainEventType = typeof(DomainEvent);

            var idProperty = domainEventType.GetProperty(
                "Id",
                BindingFlags.Public | BindingFlags.Instance);

            idProperty.Should().NotBeNull(
                "DomainEvent class should contain a public instance property named 'Id'.");
        }

        [Fact]
        public void DomainEventBaseClass_IdShouldBeGuid()
        {
            Type domainEventType = typeof(DomainEvent);

            var idProperty = domainEventType.GetProperty(
                "Id",
                BindingFlags.Public | BindingFlags.Instance);


            idProperty.Should().NotBeNull();
            idProperty!.PropertyType.Should().Be(
                typeof(Guid),
                "Id should be of type Guid.");
        }

        [Fact]
        public void DomainEventBaseClass_IdShouldBeReadonly()
        {
            Type domainEventType = typeof(DomainEvent);

            var idProperty = domainEventType.GetProperty(
                "Id",
                BindingFlags.Public | BindingFlags.Instance);

            idProperty.Should().NotBeNull();
            idProperty!.CanWrite.Should().BeFalse("Id should be read-only.");
        }

        [Fact]
        public void DomainEventBaseClass_ShouldInheritFromINotification()
        {
            Type domainEventType = typeof(DomainEvent);

            typeof(INotification).IsAssignableFrom(domainEventType)
                .Should().BeTrue(
                "DomainEvent should implement INotification.");
        }

        [Fact]
        public void DomainEvents_ShouldBeSealed()
        {
            var result = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(DomainEvent))
                .And().AreNotAbstract()
                .Should()
                .BeSealed()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void DomainEvents_ShouldBeRecords()
        {
            var types = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(DomainEvent))
                .And().AreNotAbstract()
                .GetTypes();

            foreach (var type in types)
            {
                type.GetMethods().Any(m => m.Name == "<Clone>$").Should().BeTrue();
            }
        }

        [Fact]
        public void DomainEvents_ShouldHaveNameEndingWithDomainEvent()
        {
            var result = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(DomainEvent))
                .And()
                .AreNotAbstract()
                .Should()
                .HaveNameEndingWith("DomainEvent")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }
    }
}
