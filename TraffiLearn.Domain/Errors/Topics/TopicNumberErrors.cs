using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Errors.Topics
{
    public static class TopicNumberErrors
    {
        public static Error TooSmall(int minValue) =>
            Error.Validation(
                code: "TopicNumber.TooSmall", 
                description: $"Topic number must be greater or equal to {minValue}");
    }
}
