using FluentAssertions;
using System.Reflection;
using TraffiLearn.Domain.Common.ImageUris;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;
using TraffiLearn.Testing.Shared.Factories;

namespace TraffiLearn.DomainTests.Questions
{
    public sealed class ImageUriTests
    {
        [Fact]
        public void Create_IfPassedInvalidArgs_ShouldReturnError()
        {
            Func<Result<ImageUri>>[] actions = [
                () =>
                {
                    return ImageUri.Create(null);
                },
                () =>
                {
                    return ImageUri.Create("not-valid-uri");
                },
                () =>
                {
                    return ImageUri.Create(" ");
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
            var validImageUri = QuestionFixtureFactory.CreateImageUri();

            var result = ImageUri.Create(validImageUri.Value);

            result.IsSuccess.Should().BeTrue();

            var imageUri = result.Value;

            imageUri.Value.Should().Be(validImageUri.Value);
        }

        [Fact]
        public void ImageUri_ShouldBeImmutable()
        {
            var type = typeof(ImageUri);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                property.CanWrite.Should().BeFalse($"Property {property.Name} should be read-only.");
            }
        }

        [Fact]
        public void ImageUri_ShouldInheritFromValueObject()
        {
            var type = typeof(ImageUri);

            var isValueObject = typeof(ValueObject).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("ImageUri should inherit from ValueObject.");
        }
    }
}
