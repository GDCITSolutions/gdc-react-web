namespace BE.LocalAccountabilitySystem.Entities.Response
{
    public class LockResponse
    {
        public LockResponse() { }

        public LockResponse(bool isLocked) 
        { 
            IsLocked = isLocked;
        }

        public bool IsLocked { get; set; }
    }
}
