using FluentAssertions;
using System.Reflection;
using TraffiLearn.Domain.Aggregates.Users.ValueObjects;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.DomainTests.Users
{
    public sealed class EmailTests
    {
        [Fact]
        public void Create_IfPassedInvalidArgs_ShouldReturnError()
        {
            List<Result<Email>> results = [
                Email.Create(null),
                Email.Create(" "),
                Email.Create("non-email-value"),
                Email.Create("email-like-value@"),
                Email.Create("@email.com")
            ];

            results.Should().AllSatisfy(result =>
            {
                result.IsFailure.Should().BeTrue();
                result.Error.Should().NotBe(Error.None);
            });
        }

        [Fact]
        public void Create_IfPassedValidArgs_ShouldReturnError()
        {
            var testEmail = "new-email@email.com";

            var result = Email.Create(testEmail);

            result.IsSuccess.Should().BeTrue();

            var email = result.Value;

            email.Value.Should().Be(testEmail);
        }

        [Fact]
        public void Email_ShouldBeImmutable()
        {
            var type = typeof(Email);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                property.CanWrite.Should().BeFalse($"Property {property.Name} should be read-only.");
            }
        }

        [Fact]
        public void Email_ShouldInheritFromValueObject()
        {
            var type = typeof(Email);

            var isValueObject = typeof(ValueObject).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("Email should inherit from ValueObject.");
        }
    }
}
