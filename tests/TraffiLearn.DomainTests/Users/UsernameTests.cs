using FluentAssertions;
using System.Reflection;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.DomainTests.Users
{
    public sealed class UsernameTests
    {
        [Fact]
        public void Create_IfPassedInvalidArgs_ShouldReturnError()
        {
            List<Result<Username>> results = [
                Username.Create(null),
                Username.Create(" "),
                Username.Create(new string('1', Username.MaxLength + 1))
            ];

            results.Should().AllSatisfy(result =>
            {
                result.IsFailure.Should().BeTrue();
                result.Error.Should().NotBe(Error.None);
            });
        }

        [Fact]
        public void Create_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var testUsername = "username";

            var result = Username.Create(testUsername);

            result.IsSuccess.Should().BeTrue();

            var username = result.Value;
            username.Value.Should().Be(testUsername);
        }

        [Fact]
        public void Username_ShouldBeImmutable()
        {
            var type = typeof(Username);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                property.CanWrite.Should().BeFalse($"Property {property.Name} should be read-only.");
            }
        }

        [Fact]
        public void Username_ShouldInheritFromValueObject()
        {
            var type = typeof(Username);

            var isValueObject = typeof(ValueObject).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("Username should inherit from ValueObject.");
        }
    }
}
