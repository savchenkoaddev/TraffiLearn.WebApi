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

        [Fact]
        public void ValueObjects_ShouldHaveStaticFactoryMethod()
        {
            var valueObjectTypes = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(ValueObject))
                .GetTypes();

            var failingTypes = new List<Type>();

            foreach (var valueObjectType in valueObjectTypes)
            {
                var methods = valueObjectType.GetMethods(
                    BindingFlags.Public |
                    BindingFlags.Static);

                if (!methods.Any(m => m.Name == Constants.FactoryMethodName))
                {
                    failingTypes.Add(valueObjectType);
                }
            }

            failingTypes.Should().BeEmpty();
        }

        [Fact]
        public void ValueObjects_ShouldHaveAllFieldsPrivateExceptForReadonly()
        {
            var valueObjectTypes = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(ValueObject))
                .GetTypes();

            List<Type> failingTypes = [];

            foreach (var valueObjectType in valueObjectTypes)
            {
                var fields = valueObjectType.GetFields(
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance);

                if (fields.Any(field => !field.IsPrivate && !field.IsInitOnly))
                {
                    failingTypes.Add(valueObjectType);
                }
            }

            failingTypes.Should().BeEmpty();
        }

        [Fact]
        public void ValueObjects_PropertiesSettersShouldBePrivateOrReadonly()
        {
            var valueObjectTypes = Types.InAssembly(DomainAssembly)
               .That()
               .Inherit(typeof(ValueObject))
               .GetTypes();

            var failingTypes = new List<Type>();

            foreach (var valueObjectType in valueObjectTypes)
            {
                var properties = valueObjectType.GetProperties(
                    BindingFlags.Public |
                    BindingFlags.Instance);

                foreach (var property in properties)
                {
                    var setterMethod = property.GetSetMethod(true);

                    if (setterMethod != null && !setterMethod.IsPrivate)
                    {
                        failingTypes.Add(valueObjectType);
                    }
                }
            }

            failingTypes.Should().BeEmpty();
        }
    }
}
