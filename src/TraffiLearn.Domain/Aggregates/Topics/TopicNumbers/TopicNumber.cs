﻿using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Topics.TopicNumbers
{
    public sealed class TopicNumber : ValueObject
    {
        public const int MinValue = 1;

        private TopicNumber(int value)
        {
            Value = value;
        }

        public int Value { get; }

        public static Result<TopicNumber> Create(int value)
        {
            if (value < MinValue)
            {
                return Result.Failure<TopicNumber>(
                    TopicNumberErrors.TooSmall(minValue: MinValue));
            }

            return new TopicNumber(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
