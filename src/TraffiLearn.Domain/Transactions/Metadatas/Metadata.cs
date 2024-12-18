using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Transactions.Metadatas
{
    public sealed class Metadata : ValueObject
    {
        public const int MaxLength = 5000;

        private Metadata(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<Metadata> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<Metadata>(MetadataErrors.Empty);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<Metadata>(MetadataErrors.TooLong(MaxLength));
            }

            return new Metadata(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
