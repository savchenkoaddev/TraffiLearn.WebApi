using FluentAssertions;
using FluentValidation;
using NetArchTest.Rules;

namespace TraffiLearn.ArchitectureTests.Application
{
    public sealed class ValidatorsTests : BaseTest
    {
        [Fact]
        public void Validators_ShouldBeSealed()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .Inherit(typeof(AbstractValidator<>))
                .Should()
                .BeSealed()
                .GetResult();

            if (!result.IsSuccessful)
            {
                var invalidTypes = result.FailingTypes
                    .Select(t => t.FullName)
                    .ToList();

                invalidTypes.Should().BeEmpty("The following validator types should be sealed: {0}", string.Join(", ", invalidTypes));
            }
        }

        [Fact]
        public void Validators_ShouldNotBePublic()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .Inherit(typeof(AbstractValidator<>))
                .Should()
                .NotBePublic()
                .GetResult();

            if (!result.IsSuccessful)
            {
                var invalidTypes = result.FailingTypes
                    .Select(t => t.FullName)
                    .ToList();

                invalidTypes.Should().BeEmpty("The following validator types should not be public: {0}", string.Join(", ", invalidTypes));
            }
        }

        [Fact]
        public void Validators_ShouldHaveProperNaming()
        {
            var result = Types.InAssembly(ApplicationAssembly)
                .That()
                .Inherit(typeof(AbstractValidator<>))
                .Should()
                .HaveNameEndingWith("Validator")
                .GetResult();

            if (!result.IsSuccessful)
            {
                var invalidTypes = result.FailingTypes
                    .Select(t => t.FullName)
                    .ToList();

                invalidTypes.Should().BeEmpty("The following validator types have improper naming. They should end with 'Validator': {0}", string.Join(", ", invalidTypes));
            }
        }
    }
}
