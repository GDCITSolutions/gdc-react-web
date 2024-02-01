namespace BE.LocalAccountabilitySystem.Common
{
    /// <summary>
    /// Exception primarily used during bulk import of users operations
    /// </summary>
    public class BulkImportException : Exception
    {
        public BulkImportException(string message) : base(message) { }
        public BulkImportException(string message, Exception innerException) : base(message, innerException) { }
        public BulkImportException(string message, IList<string> exceptionMessages) : base(message) 
        {
            ExceptionMessages = exceptionMessages;
        }

        public IList<string> ExceptionMessages { get; set; }
    }
}
