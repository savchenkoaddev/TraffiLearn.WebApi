using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.ArchitectureTests.Domain
{
    public sealed class EntitiesTests : BaseTest
    {
        [Fact]
        public void Entities_ShouldBeSealed()
        {
            var result = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(Entity<>))
                .Should()
                .BeSealed()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Entities_ShouldHaveParameterlessConstructor()
        {
            var entityTypes = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(Entity<>))
                .GetTypes();

            var failingTypes = new List<Type>();

            foreach (var entityType in entityTypes)
            {
                var ctors = entityType.GetConstructors(
                    BindingFlags.NonPublic |
                    BindingFlags.Instance);

                if (!ctors.Any(c => c.IsPrivate && c.GetParameters().Length == 0))
                {
                    failingTypes.Add(entityType);
                }
            }

            failingTypes.Should().BeEmpty();
        }

    }
}
