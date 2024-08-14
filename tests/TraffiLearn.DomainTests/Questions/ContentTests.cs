using FluentAssertions;
using System.Reflection;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;

namespace TraffiLearn.DomainTests.Questions
{
    public sealed class ContentTests
    {
        [Fact]
        public void Create_IfPassedInvalidArgs_ShouldReturnError()
        {
            Func<Result<QuestionContent>>[] actions = [
                () =>
                {
                    return QuestionContent.Create(null);
                },
                () =>
                {
                    return QuestionContent.Create(
                        new string('1', QuestionContent.MaxLength + 1));
                },
                () =>
                {
                    return QuestionContent.Create(" ");
                },
            ];

            actions.Should().AllSatisfy(action =>
            {
                action().IsFailure.Should().BeTrue();
                action().Error.Should().NotBe(Error.None);
            });
        }

        [Fact]
        public void Create_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var value = "string";
            var result = QuestionContent.Create(value);

            result.IsSuccess.Should().BeTrue();

            var content = result.Value;

            content.Value.Should().Be(value);
        }

        [Fact]
        public void Content_ShouldBeImmutable()
        {
            var type = typeof(QuestionContent);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                property.CanWrite.Should().BeFalse($"Property {property.Name} should be read-only.");
            }
        }

        [Fact]
        public void Content_ShouldInheritFromValueObject()
        {
            var type = typeof(QuestionContent);

            var isValueObject = typeof(ValueObject).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("Content should inherit from ValueObject.");
        }
    }
}
