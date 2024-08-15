using FluentAssertions;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.DomainTests.Primitives
{
    public sealed class ValueObjectTests
    {
        [Fact]
        public void ValueObjects_WithSameValues_ShouldBeEqual()
        {
            var address1 = new TestValueObject("123 Main St", "Springfield");
            var address2 = new TestValueObject("123 Main St", "Springfield");

            address1.Should().Be(address2);
        }

        [Fact]
        public void ValueObjects_WithDifferentValues_ShouldNotBeEqual()
        {
            var address1 = new TestValueObject("123 Main St", "Springfield");
            var address2 = new TestValueObject("456 Elm St", "Springfield");

            address1.Should().NotBe(address2);
        }

        [Fact]
        public void ValueObjects_WithDifferentTypes_ShouldNotBeEqual()
        {
            var address = new TestValueObject("123 Main St", "Springfield");
            var otherObject = new object();

            address.Equals(otherObject).Should().BeFalse();
        }

        [Fact]
        public void GetHashCode_ForSameValues_ShouldBeConsistent()
        {
            var address1 = new TestValueObject("123 Main St", "Springfield");
            var address2 = new TestValueObject("123 Main St", "Springfield");

            address1.GetHashCode().Should().Be(address2.GetHashCode());
        }

        [Fact]
        public void GetHashCode_ForDifferentValues_ShouldBeDifferent()
        {
            var address1 = new TestValueObject("123 Main St", "Springfield");
            var address2 = new TestValueObject("456 Elm St", "Springfield");

            var hashCode1 = address1.GetHashCode();
            var hashCode2 = address2.GetHashCode();

            hashCode1.Should().NotBe(hashCode2);
        }

        private sealed class TestValueObject : ValueObject
        {
            public TestValueObject(
                string street,
                string city)
            {
                Street = street;
                City = city;
            }

            public string Street { get; }
            public string City { get; }

            public override IEnumerable<object> GetAtomicValues()
            {
                yield return Street;
                yield return City;
            }
        }
    }
}
