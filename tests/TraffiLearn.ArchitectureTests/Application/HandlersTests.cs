using MediatR;
using NetArchTest.Rules;

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
    }
}
