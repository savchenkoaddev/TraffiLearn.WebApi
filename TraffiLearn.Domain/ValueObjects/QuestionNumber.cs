using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.ValueObjects
{
    public sealed class QuestionNumber : ValueObject
    {
        public const int MinValue = 1;

        private QuestionNumber(int value)
        {
            Value = value;
        }

        public int Value { get; init; }

        public static QuestionNumber Create(int value)
        {
            if (value < MinValue)
            {
                throw new ArgumentException($"Question number cannot be less than {MinValue} characters.");
            }

            return new QuestionNumber(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
