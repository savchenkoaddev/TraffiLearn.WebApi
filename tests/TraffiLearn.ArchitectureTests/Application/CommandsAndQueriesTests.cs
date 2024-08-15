using FluentAssertions;
using MediatR;
using NetArchTest.Rules;

namespace TraffiLearn.ArchitectureTests.Application
{
    public sealed class CommandsAndQueriesTests : BaseTest
    {
        [Fact]
        public void CommandsAndQueries_ShouldBeSealed()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(IRequest))
                .Or()
                .ImplementInterface(typeof(IRequest<>))
                .Should()
                .BeSealed()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void CommandsAndQueries_ShouldHaveNameEndingWithCommandOrQuery()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(IRequest))
                .Or()
                .ImplementInterface(typeof(IRequest<>))
                .Should()
                .HaveNameEndingWith("Command")
                .Or()
                .HaveNameEndingWith("Query")
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Queries_ShouldHaveReturnType()
        {
            var types = Types.InAssembly(ApplicationAssembly)
                .That()
                .HaveNameEndingWith("Query")
                .And()
                .ImplementInterface(typeof(IRequest))
                .GetTypes();

            types.Should().BeEmpty();
        }

        [Fact]
        public void QueriesAndCommands_ShouldBePublic()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(IRequest))
                .Or()
                .ImplementInterface(typeof(IRequest<>))
                .Should()
                .BePublic()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void QueriesAndCommands_ShouldHaveOnlyNullableMembers()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(IRequest))
                .Or()
                .ImplementInterface(typeof(IRequest<>))
                .Should()
                .OnlyHaveNullableMembers()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void QueriesAndCommands_ShouldBeRecords()
        {
            var types = Types.InAssembly(ApplicationAssembly)
                .That()
                .ImplementInterface(typeof(IRequest))
                .Or()
                .ImplementInterface(typeof(IRequest<>))
                .GetTypes();

            foreach (var type in types)
            {
                type.GetMethods().Any(m => m.Name == "<Clone>$").Should().BeTrue();
            }
        }
    }
}
