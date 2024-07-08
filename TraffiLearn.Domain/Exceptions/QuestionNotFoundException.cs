namespace TraffiLearn.Domain.Exceptions
{
    public sealed class QuestionNotFoundException : Exception
    {
        public QuestionNotFoundException() : base("Question with the provided id has not been found.")
        { }

        public QuestionNotFoundException(Guid questionId) : base($"Question with the '{questionId}' id has not been found.")
        { }
    }
}
