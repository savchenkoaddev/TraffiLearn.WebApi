using FluentAssertions;
using NetArchTest.Rules;
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
    }
}
