using FluentAssertions;
using NetArchTest.Rules;

namespace TraffiLearn.ArchitectureTests
{
    public sealed class LayerTests : BaseTest
    {
        [Fact]
        public void Domain_ShouldNotHaveDependencyOnApplicationLayer()
        {
            var result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(
                    ApplicationAssembly.FullName)
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Domain_ShouldNotHaveDependencyOnInfrastructureLayer()
        {
            var result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(
                    InfrastructureAssembly.FullName)
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Domain_ShouldNotHaveDependencyOnPresentationLayer()
        {
            var result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOn(
                    WebApiAssembly.FullName)
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Domain_ShouldNotHaveDependencyOnTests()
        {
            var result = Types.InAssembly(DomainAssembly)
                .Should()
                .NotHaveDependencyOnAny(
                    DomainTestsAssembly.FullName,
                    GetType().Assembly.FullName)
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Application_ShouldNotHaveDependencyOnInfrastructureLayer()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .Should()
                .NotHaveDependencyOn(
                    InfrastructureAssembly.FullName)
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Application_ShouldNotHaveDependencyOnPresentationLayer()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .Should()
                .NotHaveDependencyOn(
                    WebApiAssembly.FullName)
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Application_ShouldNotHaveDependencyOnTests()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .Should()
                .NotHaveDependencyOnAny(
                    DomainTestsAssembly.FullName,
                    GetType().Assembly.FullName)
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Infrastructure_ShouldNotHaveDependencyOnPresentationLayer()
        {
            var result = Types.InAssembly(InfrastructureAssembly)
                .Should()
                .NotHaveDependencyOn(
                    WebApiAssembly.FullName)
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Infrastructure_ShouldNotHaveDependencyOnTests()
        {
            var result = Types.InAssembly(InfrastructureAssembly)
                 .Should()
                 .NotHaveDependencyOnAny(
                     DomainTestsAssembly.FullName,
                     GetType().Assembly.FullName)
                 .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Presentation_ShouldNotHaveDependencyOnTests()
        {
            var result = Types.InAssembly(WebApiAssembly)
                 .Should()
                 .NotHaveDependencyOnAny(
                     DomainTestsAssembly.FullName,
                     GetType().Assembly.FullName)
                 .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }
    }
}
