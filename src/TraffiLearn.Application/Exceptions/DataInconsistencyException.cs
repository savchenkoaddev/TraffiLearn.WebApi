namespace TraffiLearn.Application.Exceptions
{
    public sealed class DataInconsistencyException : Exception
    {
        public DataInconsistencyException()
            : base("Critical data inconsistency failure detected. " +
                   "The state of the data does not match the expected integrity constraints.")
        { }
    }
}
