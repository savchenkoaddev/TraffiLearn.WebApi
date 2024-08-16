using FluentAssertions;
using System.Reflection;
using TraffiLearn.Domain.Aggregates.Comments.ValueObjects;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.DomainTests.Comments
{
    public sealed class ContentTests
    {
        [Fact]
        public void Create_IfPassedInvalidArgs_ShouldReturnError()
        {
            List<Result<CommentContent>> cases = [
                CommentContent.Create(null),

                CommentContent.Create(" "),

                CommentContent.Create(
                    new string('c', CommentContent.MaxLength + 1))
            ];

            cases.Should().AllSatisfy(action =>
            {
                action.IsFailure.Should().BeTrue();
                action.Error.Should().NotBe(Error.None);
            });
        }

        [Fact]
        public void Create_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var value = "Comment";
            var result = CommentContent.Create(value);

            result.IsSuccess.Should().BeTrue();

            var content = result.Value;

            content.Value.Should().Be(value);
        }

        [Fact]
        public void Content_ShouldBeImmutable()
        {
            var type = typeof(CommentContent);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                property.CanWrite.Should().BeFalse($"Property {property.Name} should be read-only.");
            }
        }

        [Fact]
        public void Content_ShouldInheritFromValueObject()
        {
            var type = typeof(CommentContent);

            var isValueObject = typeof(ValueObject).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("CommentContent should inherit from ValueObject.");
        }
    }
}
