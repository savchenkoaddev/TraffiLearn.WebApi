using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.ValueObjects
{
    public sealed class TicketNumber : ValueObject
    {
        public const int MinValue = 1;

        private TicketNumber(int value)
        {
            Value = value;
        }

        public int Value { get; init; }

        public static TicketNumber Create(int value)
        {
            if (value < MinValue)
            {
                throw new ArgumentException($"Ticket number cannot be less than {MinValue} characters.");
            }

            return new TicketNumber(value);
        }

        public override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}
