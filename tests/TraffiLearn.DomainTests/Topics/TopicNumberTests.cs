﻿using FluentAssertions;
using System.Reflection;
using TraffiLearn.Domain.Topics.TopicNumbers;
using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.DomainTests.Topics
{
    public sealed class TopicNumberTests
    {
        [Fact]
        public void Create_IfPassedInvalidArgs_ShouldReturnError()
        {
            int tooSmallNumber = TopicNumber.MinValue - 1;

            var result = TopicNumber.Create(tooSmallNumber);

            result.IsFailure.Should().Be(true);

            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void Create_IfPassedValidArgs_ShouldReturnValidTopicNumber()
        {
            int validNumber = TopicNumber.MinValue;

            var result = TopicNumber.Create(validNumber);

            result.IsSuccess.Should().Be(true);

            var topicNumber = result.Value;

            topicNumber.Value.Should().Be(validNumber);
        }

        [Fact]
        public void TopicNumber_ShouldBeImmutable()
        {
            var type = typeof(TopicNumber);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                property.CanWrite.Should().BeFalse($"Property {property.Name} should be read-only.");
            }
        }

        [Fact]
        public void TopicNumber_ShouldInheritFromValueObject()
        {
            var type = typeof(TopicNumber);

            var isValueObject = typeof(ValueObject).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("TopicNumber should inherit from ValueObject.");
        }
    }
}
