namespace TraffiLearn.Domain.Exceptions
{
    public sealed class AllAnswersAreIncorrectException : Exception
    {
        public AllAnswersAreIncorrectException() : base("There are no correct answers in the provided answers.")
        { }
    }
}
