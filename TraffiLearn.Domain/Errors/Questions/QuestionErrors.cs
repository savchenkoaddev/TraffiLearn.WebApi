using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Errors.Questions
{
    public static class QuestionErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "Question.NotFound",
                description: "Question has not been found.");
    }
}
