using FluentAssertions;
using System.Reflection;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;

namespace TraffiLearn.DomainTests.Questions
{
    public sealed class ExplanationTests
    {
        [Fact]
        public void Create_IfPassedInvalidArgs_ShouldReturnError()
        {
            Func<Result<QuestionExplanation>>[] actions = [
                () =>
                {
                    return QuestionExplanation.Create(null);
                },
                () =>
                {
                    return QuestionExplanation.Create(
                        new string('1', QuestionExplanation.MaxLength + 1));
                },
                () =>
                {
                    return QuestionExplanation.Create(" ");
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
            var result = QuestionExplanation.Create(value);

            result.IsSuccess.Should().BeTrue();

            var content = result.Value;

            content.Value.Should().Be(value);
        }

        [Fact]
        public void Explanation_ShouldBeImmutable()
        {
            var type = typeof(QuestionExplanation);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                property.CanWrite.Should().BeFalse($"Property {property.Name} should be read-only.");
            }
        }

        [Fact]
        public void Explanation_ShouldInheritFromValueObject()
        {
            var type = typeof(QuestionExplanation);
            
            var isValueObject = typeof(ValueObject).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("Explanation should inherit from ValueObject.");
        }
    }
}
