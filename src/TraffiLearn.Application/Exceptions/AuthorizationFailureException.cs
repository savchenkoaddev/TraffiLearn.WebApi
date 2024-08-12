namespace TraffiLearn.Application.Exceptions
{
    public sealed class AuthorizationFailureException : Exception
    {
        public AuthorizationFailureException()
            : base("Critical authorization failure.")
        { }

        public AuthorizationFailureException(string message)
            : base(message)
        { }
    }
}
