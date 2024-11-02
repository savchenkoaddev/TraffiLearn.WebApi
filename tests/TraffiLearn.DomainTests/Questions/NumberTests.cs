using FluentAssertions;
using System.Reflection;
using TraffiLearn.Domain.Questions.QuestionNumbers;
using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.DomainTests.Questions
{
    public sealed class NumberTests
    {
        [Fact]
        public void Create_IfPassedInvalidArgs_ShouldReturnError()
        {
            var result = QuestionNumber.Create(QuestionNumber.MinValue - 1);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void Create_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var value = QuestionNumber.MinValue;
            var result = QuestionNumber.Create(value);

            result.IsSuccess.Should().BeTrue();

            var content = result.Value;

            content.Value.Should().Be(value);
        }

        [Fact]
        public void Number_ShouldBeImmutable()
        {
            var type = typeof(QuestionNumber);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                property.CanWrite.Should().BeFalse($"Property {property.Name} should be read-only.");
            }
        }

        [Fact]
        public void Number_ShouldInheritFromValueObject()
        {
            var type = typeof(QuestionNumber);

            var isValueObject = typeof(ValueObject).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("Number should inherit from ValueObject.");
        }
    }
}
