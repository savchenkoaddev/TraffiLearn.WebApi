﻿using TraffiLearn.Domain.Aggregates.ServiceCenters.Errors;
using TraffiLearn.Domain.Primitives;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Aggregates.ServiceCenters.ValueObjects
{
    public sealed class BuildingNumber : ValueObject
    {
        public const int MaxLength = 25;

        private BuildingNumber(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<BuildingNumber> Create(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<BuildingNumber>(
                    BuildingNumberErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<BuildingNumber>(
                    BuildingNumberErrors.TooLong(allowedLength: MaxLength));
            }

            return new BuildingNumber(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
