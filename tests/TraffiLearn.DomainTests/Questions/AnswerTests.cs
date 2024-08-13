using FluentAssertions;
using System.Reflection;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Questions;

namespace TraffiLearn.DomainTests.Questions
{
    public sealed class AnswerTests
    {
        [Fact]
        public void Create_IfPassedInvalidArgs_ShouldReturnError()
        {
            Func<Result<Answer>>[] actions = [
                () =>
                {
                    return Answer.Create(
                        null,
                        true);
                },
                () =>
                {
                    return Answer.Create(
                        " ",
                        false);
                },
                () =>
                {
                    return Answer.Create(
                        new string('1', Answer.MaxTextLength + 1),
                        true);
                }
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
            var text = "Value";
            var isCorrect = true;

            var result = Answer.Create(
                text,
                isCorrect);

            result.IsSuccess.Should().BeTrue();

            var answer = result.Value;

            answer.Text.Should().Be(text);
            answer.IsCorrect.Should().Be(isCorrect);
        }

        [Fact]
        public void Answer_ShouldBeComparedByTextOnly()
        {
            var text = "Value";

            var answer = Answer.Create(
                text,
                true).Value;

            var sameAnswer = Answer.Create(
                text,
                false).Value;

            answer.Should().Be(sameAnswer);
        }

        [Fact]
        public void Answer_ShouldBeImmutable()
        {
            var type = typeof(Answer);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                property.CanWrite.Should().BeFalse($"Property {property.Name} should be read-only.");
            }
        }

        [Fact]
        public void Answer_ShouldInheritFromValueObject()
        {
            var type = typeof(Answer);

            var isValueObject = typeof(ValueObject).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("Answer should inherit from ValueObject.");
        }
    }
}
