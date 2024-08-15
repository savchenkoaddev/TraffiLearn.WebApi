using FluentAssertions;
using MediatR;
using NetArchTest.Rules;
using System.Reflection;

namespace TraffiLearn.ArchitectureTests.Application
{
    public sealed class HandlersTests : BaseTest
    {
        [Fact]
        public void Handlers_ShouldBeSealed()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(IRequestHandler<>))
                .Or()
                .ImplementInterface(typeof(IRequestHandler<,>))
                .Should()
                .BeSealed()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Handlers_ShouldNotBePublic()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(IRequestHandler<>))
                .Or()
                .ImplementInterface(typeof(IRequestHandler<,>))
                .Should()
                .NotBePublic()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Handlers_ShouldOnlyHaveReadonlyFields()
        {
            var types = Types.InAssembly(ApplicationAssembly)
               .That()
               .ImplementInterface(typeof(IRequestHandler<>))
               .Or()
               .ImplementInterface(typeof(IRequestHandler<,>))
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
        public void Handlers_ShouldNotHavePublicFieldsAndProperties()
        {
            var types = Types.InAssembly(ApplicationAssembly)
               .That()
               .ImplementInterface(typeof(IRequestHandler<>))
               .Or()
               .ImplementInterface(typeof(IRequestHandler<,>))
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
        public void Handlers_ShouldHaveProperNaming()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(IRequestHandler<>))
                .Or()
                .ImplementInterface(typeof(IRequestHandler<,>))
                .Should()
                .HaveNameEndingWith("CommandHandler")
                .Or()
                .HaveNameEndingWith("QueryHandler")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Handlers_ShouldNotHaveStaticMembers()
        {
            var types = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(IRequestHandler<>))
                .Or()
                .ImplementInterface(typeof(IRequestHandler<,>))
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
