namespace BE.LocalAccountabilitySystem.Common
{
    /// <summary>
    /// Related to any exceptions that occur when working with users
    /// </summary>
    public class UserException : Exception
    {
        public UserException(string message) : base(message) { }
    }
}
