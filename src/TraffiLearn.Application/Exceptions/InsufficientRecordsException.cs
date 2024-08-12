namespace TraffiLearn.Application.Exceptions
{
    public sealed class InsufficientRecordsException : Exception
    {
        public InsufficientRecordsException()
            : base($"Unable to perform action due to records insufficiency in the storage.")
        { }

        public InsufficientRecordsException(
            int requiredRecords,
            int availableRecords)
            : base($"Unable to perform action due to records insufficiency in the storage. Required: {requiredRecords}, Available: {availableRecords}.")
        { }
    }
}
