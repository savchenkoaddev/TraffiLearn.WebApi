using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;
using TraffiLearn.SharedKernel.Primitives;

namespace TraffiLearn.ArchitectureTests.Infrastructure
{
    public sealed class RepositoriesTests : BaseTest
    {
        [Fact]
        public void Repositories_ShouldBeSealed()
        {
            var result = Types.InAssembly(InfrastructureAssembly)
                .That()
                .ImplementInterface(typeof(IRepositoryMarker))
                .Should()
                .BeSealed()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Repositories_ShouldNotBePublic()
        {
            var result = Types.InAssembly(InfrastructureAssembly)
                .That()
                .ImplementInterface(typeof(IRepositoryMarker))
                .Should()
                .NotBePublic()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Repositories_ShouldOnlyHaveReadonlyFields()
        {
            var types = Types.InAssembly(InfrastructureAssembly)
                .That()
                .ImplementInterface(typeof(IRepositoryMarker))
                .GetTypes();

            foreach (var type in types)
            {
                var fields = type.GetFields(
                    BindingFlags.NonPublic |
                    BindingFlags.Instance |
                    BindingFlags.Static)
                    .Where(field => !field.IsInitOnly)
                    .Should()
                    .BeEmpty();
            }
        }

        [Fact]
        public void Repositories_ShouldNotHavePublicFieldsAndProperties()
        {
            var types = Types.InAssembly(InfrastructureAssembly)
                .That()
                .ImplementInterface(typeof(IRepositoryMarker))
                .GetTypes();

            foreach (var type in types)
            {
                var publicFields = type.GetFields(
                    BindingFlags.Public |
                    BindingFlags.Instance);

                publicFields.Should().BeEmpty();

                var publicProperties = type.GetProperties();

                publicProperties.Should().BeEmpty();
            }
        }

        [Fact]
        public void Repositories_ShouldHaveNameEndingWithRepository()
        {
            var result = Types.InAssembly(InfrastructureAssembly)
                .That()
                .ImplementInterface(typeof(IRepositoryMarker))
                .Should()
                .HaveNameEndingWith("Repository")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Repositories_ShouldNotHaveStaticMemebers()
        {
            var types = Types.InAssembly(InfrastructureAssembly)
                .That()
                .ImplementInterface(typeof(IRepositoryMarker))
                .GetTypes();

            foreach (var type in types)
            {
                var staticFields = type.GetFields(
                    BindingFlags.NonPublic |
                    BindingFlags.Public |
                    BindingFlags.Static);

                var staticProperties = type.GetProperties(
                    BindingFlags.NonPublic |
                    BindingFlags.Public |
                    BindingFlags.Static);

                staticFields.Should().BeEmpty();
                staticProperties.Should().BeEmpty();
            }
        }
    }
}
