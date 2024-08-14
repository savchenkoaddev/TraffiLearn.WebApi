using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.ArchitectureTests.Domain
{
    public sealed class ValueObjectsTests : BaseTest
    {
        [Fact]
        public void ValueObjects_ShouldBeSealed()
        {
            var result = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(ValueObject))
                .Should()
                .BeSealed()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void ValueObjects_ShouldNotHavePublicConstructors()
        {
            var valueObjectTypes = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(ValueObject))
                .GetTypes();

            var failingTypes = new List<Type>();

            foreach (var valueObjectType in valueObjectTypes)
            {
                var ctors = valueObjectType.GetConstructors(
                    BindingFlags.Public |
                    BindingFlags.Instance);

                if (ctors.Length > 0)
                {
                    failingTypes.Add(valueObjectType);
                }
            }

            failingTypes.Should().BeEmpty();
        }
    }
}
