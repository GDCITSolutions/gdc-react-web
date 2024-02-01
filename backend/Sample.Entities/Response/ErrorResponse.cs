namespace BE.LocalAccountabilitySystem.Entities.Response
{
    /// <summary>
    /// For returning errors in an easily digestable format
    /// </summary>
    public class ErrorResponse
    {
        public ErrorResponse() { }

        public ErrorResponse(string message) 
        {
            Message = message;
        }

        public ErrorResponse(IList<string> messages)
        {
            Messages = messages;
        }

        public string Message { get; set; }

        public IList<string> Messages { get; set; }
    }
}
