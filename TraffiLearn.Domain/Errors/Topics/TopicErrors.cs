using TraffiLearn.Domain.Primitives;

namespace TraffiLearn.Domain.Errors.Topics
{
    public static class TopicErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "Topic.NotFound",
                description: "Topic has not been found.");
    }
}
