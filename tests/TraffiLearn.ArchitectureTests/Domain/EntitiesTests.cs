using FluentAssertions;
using NetArchTest.Rules;
using System.Reflection;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.ArchitectureTests.Domain
{
    public sealed class EntitiesTests : BaseTest
    {
        [Fact]
        public void Entities_ShouldBeSealed()
        {
            var result = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(Entity<>))
                .Should()
                .BeSealed()
                .GetResult();

            result.IsSuccessful.Should().BeTrue();
        }

        [Fact]
        public void Entities_ShouldHaveNonPublicParameterlessConstructor()
        {
            var entityTypes = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(Entity<>))
                .GetTypes();

            var failingTypes = new List<Type>();

            foreach (var entityType in entityTypes)
            {
                var ctors = entityType.GetConstructors(
                    BindingFlags.NonPublic |
                    BindingFlags.Instance);

                if (!ctors.Any(c => c.IsPrivate && c.GetParameters().Length == 0))
                {
                    failingTypes.Add(entityType);
                }
            }

            failingTypes.Should().BeEmpty();
        }

        [Fact]
        public void Entities_ShouldNotHavePublicConstructors()
        {
            var entityTypes = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(Entity<>))
                .GetTypes();

            var failingTypes = new List<Type>();

            foreach (var entityType in entityTypes)
            {
                var ctors = entityType.GetConstructors(
                    BindingFlags.Public |
                    BindingFlags.Instance);

                if (ctors.Length > 0)
                {
                    failingTypes.Add(entityType);
                }
            }

            failingTypes.Should().BeEmpty();
        }

        [Fact]
        public void Entities_ShouldHaveStaticFactoryMethod()
        {
            var entityTypes = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(Entity<>))
                .GetTypes();

            var failingTypes = new List<Type>();

            foreach (var entityType in entityTypes)
            {
                var methods = entityType.GetMethods(
                    BindingFlags.Public |
                    BindingFlags.Static);

                if (!methods.Any(m => m.Name == Constants.FactoryMethodName))
                {
                    failingTypes.Add(entityType);
                }
            }

            failingTypes.Should().BeEmpty();
        }

        [Fact]
        public void Entities_PropertiesSettersShouldBePrivate()
        {
            var entityTypes = Types.InAssembly(DomainAssembly)
               .That()
               .Inherit(typeof(Entity<>))
               .GetTypes();

            var failingTypes = new List<Type>();

            foreach (var entityType in entityTypes)
            {
                var properties = entityType.GetProperties(
                    BindingFlags.Public |
                    BindingFlags.Instance);

                foreach (var property in properties)
                {
                    var setterMethod = property.GetSetMethod(true);

                    if (setterMethod != null && !setterMethod.IsPrivate)
                    {
                        failingTypes.Add(entityType);
                    }
                }
            }

            failingTypes.Should().BeEmpty();
        }

        [Fact]
        public void Entities_ShouldHaveOnlyReadOnlyCollections()
        {
            var entityTypes = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(Entity<>))
                .GetTypes();

            var failingTypes = new List<Type>();

            foreach (var entityType in entityTypes)
            {
                var properties = entityType.GetProperties(
                    BindingFlags.Public |
                    BindingFlags.Instance);

                foreach (var property in properties)
                {
                    if (property.PropertyType.IsAssignableTo(typeof(System.Collections.IEnumerable)))
                    {
                        var isPublicGetter = property.GetGetMethod()?.IsPublic ?? false;

                        if (isPublicGetter)
                        {
                            var genericTypeDefinition = property.PropertyType.GetGenericTypeDefinition();

                            if (!genericTypeDefinition.IsAssignableTo(typeof(IReadOnlyCollection<>)))
                            {
                                failingTypes.Add(entityType);
                            }
                        }
                    }
                }
            }

            failingTypes.Should().BeEmpty();
        }

        [Fact]
        public void Entities_ShouldHaveAllFieldsPrivateExceptForReadonly()
        {
            var entityTypes = Types.InAssembly(DomainAssembly)
                .That()
                .Inherit(typeof(Entity<>))
                .GetTypes();

            List<Type> failingTypes = [];

            foreach (var entityType in entityTypes)
            {
                var fields = entityType.GetFields(
                    BindingFlags.Public |
                    BindingFlags.NonPublic |
                    BindingFlags.Instance);

                if (fields.Any(field => !field.IsPrivate && !field.IsInitOnly))
                {
                    failingTypes.Add(entityType);
                }
            }

            failingTypes.Should().BeEmpty();
        }
    }
}
