﻿using FluentAssertions;
using System.Reflection;
using TraffiLearn.Domain.Tickets.TicketNumbers;
using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.DomainTests.Tickets
{
    public sealed class TicketNumberTests
    {
        [Fact]
        public void Create_IfPassedInvalidArgs_ShouldReturnError()
        {
            var result = TicketNumber.Create(TicketNumber.MinValue - 1);

            result.IsFailure.Should().BeTrue();
            result.Error.Should().NotBe(Error.None);
        }

        [Fact]
        public void Create_IfPassedValidArgs_ShouldBeSuccesful()
        {
            var result = TicketNumber.Create(TicketNumber.MinValue);

            result.IsSuccess.Should().BeTrue();

            var number = result.Value;

            number.Value.Should().Be(TicketNumber.MinValue);
        }

        [Fact]
        public void TicketNumber_ShouldBeImmutable()
        {
            var type = typeof(TicketNumber);
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var property in properties)
            {
                property.CanWrite.Should().BeFalse($"Property {property.Name} should be read-only.");
            }
        }

        [Fact]
        public void TicketNumber_ShouldInheritFromValueObject()
        {
            var type = typeof(TicketNumber);

            var isValueObject = typeof(ValueObject).IsAssignableFrom(type);
            isValueObject.Should().BeTrue("TicketNumber should inherit from ValueObject.");
        }
    }
}
