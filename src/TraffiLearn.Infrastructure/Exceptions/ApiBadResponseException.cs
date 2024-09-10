namespace TraffiLearn.Infrastructure.Exceptions
{
    public sealed class ApiBadResponseException : Exception
    {
        public ApiBadResponseException() : base("Api returned bad response.")
        { }

        public ApiBadResponseException(string apiName) : base($"{apiName} returned bad response.")
        { }
    }
}
