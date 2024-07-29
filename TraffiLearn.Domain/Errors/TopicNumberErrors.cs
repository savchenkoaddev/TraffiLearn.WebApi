using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Errors
{
    public static class TopicNumberErrors
    {
        public static Error TooSmall(int minValue) =>
            Error.Validation("TopicNumber.TooSmall", $"Topic number must be greater or equal to {minValue}");
    }
}
