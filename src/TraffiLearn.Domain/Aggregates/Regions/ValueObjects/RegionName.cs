﻿using TraffiLearn.Domain.Aggregates.Regions.Errors;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.Regions.ValueObjects
{
    public sealed class RegionName : ValueObject
    {
        public const int MaxLength = 100;

        private RegionName(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<RegionName> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<RegionName>(
                    RegionNameErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<RegionName>(
                    RegionNameErrors.TooLong(allowedLength: MaxLength));
            }

            return new RegionName(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
