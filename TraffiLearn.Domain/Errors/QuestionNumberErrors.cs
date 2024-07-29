using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Errors
{
    public static class QuestionNumberErrors
    {
        public static Error TooSmall(int minValue) =>
            Error.Validation("QuestionNumber.TooSmall", $"Question number must be greater or equal to {minValue}");
    }
}
