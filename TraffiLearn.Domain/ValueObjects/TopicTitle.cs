using TraffiLearn.Domain.Errors;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.ValueObjects
{
    public sealed class TopicTitle : ValueObject
    {
        public const int MaxLength = 300;

        private TopicTitle(string value)
        {
            Value = value;
        }

        public string Value { get; init; }

        public static Result<TopicTitle> Create(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return Result.Failure<TopicTitle>(
                    TopicTitleErrors.EmptyText);
            }

            if (value.Length > MaxLength)
            {
                return Result.Failure<TopicTitle>(
                    TopicTitleErrors.TooLongText(allowedLength: MaxLength));
            }

            return new TopicTitle(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
