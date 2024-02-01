namespace BE.LocalAccountabilitySystem.Common.Exceptions
{
    /// <summary>
    /// If an entity is in a locked state, throw this
    /// </summary>
    public class LockedEntityException : Exception
    {
        public LockedEntityException(string message) : base(message) { }
    }
}
