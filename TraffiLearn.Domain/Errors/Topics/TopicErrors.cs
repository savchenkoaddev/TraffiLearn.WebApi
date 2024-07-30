using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Domain.Errors.Topics
{
    public static class TopicErrors
    {
        public static readonly Error NotFound =
            Error.NotFound(
                code: "Topic.NotFound",
                description: "Topic has not been found.");

        public static readonly Error QuestionAlreadyAdded =
            Error.OperationFailure(
                code: "Topic.QuestionAlreadyAdded",
                description: "The topic already contains the question.");

        public static readonly Error QuestionNotFound =
           Error.NotFound(
               code: "Topic.QuestionNotFound",
               description: "The topic does not contain the provided question.");
    }
}
