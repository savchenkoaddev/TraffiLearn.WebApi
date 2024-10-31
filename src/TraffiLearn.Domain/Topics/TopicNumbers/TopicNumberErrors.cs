using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Domain.Topics.TopicNumbers
{
    public static class TopicNumberErrors
    {
        public static Error TooSmall(int minValue) =>
            Error.Validation(
                code: "TopicNumber.TooSmall",
                description: $"Topic number must be greater or equal to {minValue}");
    }
}
