using FluentAssertions;
using NetArchTest.Rules;

namespace TraffiLearn.ArchitectureTests
{
    public sealed class ExceptionsTests : BaseTest
    {
        [Fact]
        public void Exceptions_ShouldHaveProperNaming()
        {
            var result = Types.InAssemblies(
                [
                    DomainAssembly,
                    ApplicationAssembly,
                    InfrastructureAssembly,
                    WebApiAssembly
                ])
                .That()
                .Inherit(typeof(Exception))
                .Should()
                .HaveNameEndingWith("Exception")
                .GetResult();

            if (!result.IsSuccessful)
            {
                var invalidTypes = result.FailingTypes
                    .Select(t => t.FullName)
                    .ToList();

                invalidTypes.Should().BeEmpty(
                    "The following exception types should end with 'Exception': {0}",
                    string.Join(", ", invalidTypes));
            }
        }
    }
}
