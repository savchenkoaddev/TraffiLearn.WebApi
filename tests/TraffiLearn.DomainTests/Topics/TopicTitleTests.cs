using FluentAssertions;
using System.Reflection;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Domain.ValueObjects.Topics;

namespace TraffiLearn.DomainTests.Topics
{
    public sealed class TopicTitleTests
    {
        [Fact]
        public void TopicTitle_IfPassedInvalidArgs_ShouldReturnError()
        {
            var nullResult = TopicTitle.Create(null);

            nullResult.IsFailure.Should().BeTrue();
            nullResult.Error.Should().NotBe(Error.None);

            var tooLongMessage = new string('x', TopicTitle.MaxLength + 1);

            var longMessageResult = TopicTitle.Create(tooLongMessage);

            longMessageResult.IsFailure.Should().BeTrue();
            longMessageResult.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void TopicTitle_IfPassedValidArgs_ShouldReturnValidTopicTitle()
        {
            var title = new string('x', TopicTitle.MaxLength - 1);

            var result = TopicTitle.Create(title);

            result.IsSuccess.Should().BeTrue();

            var topicTitle = result.Value;

            topicTitle.Value.Should().Be(title);

            topicTitle.GetAtomicValues().Should().HaveCount(1);
            topicTitle.GetAtomicValues().First().Should().Be(title);
        }

        [Fact]
        public void TopicTitle_ShouldBeImmutable()
        {
            var type = typeof(TopicTitle);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                property.CanWrite.Should().BeFalse($"Property {property.Name} should be read-only.");
            }
        }

        [Fact]
        public void TopicTitle_ShouldInheritFromValueObject()
        {
            var type = typeof(TopicTitle);

            var isValueObject = typeof(ValueObject).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("TopicTitle should inherit from ValueObject.");
        }
    }
}
