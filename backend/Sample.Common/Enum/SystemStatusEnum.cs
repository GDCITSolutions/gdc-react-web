namespace BE.LocalAccountabilitySystem.Common.Enum
{
    public enum SystemStatusEnum
    {
        Active = 1,
        Inactive = 2,
        Removed = 3,
        Locked = 4
    }

    public static class SystemStatusEnumExtensions 
    {
        public static int AsInt(this SystemStatusEnum status) 
        {
            return (int)status;
        }
    }
}
