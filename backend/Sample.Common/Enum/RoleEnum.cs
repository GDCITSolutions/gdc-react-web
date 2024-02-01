namespace BE.LocalAccountabilitySystem.Common.Enum
{
    public enum RoleEnum
    {
        DistrictAdmin = 1,
        SchoolAdmin = 2,
        Faculty = 3,
        Parent = 4,
        Student = 5
    }

    public static class RoleEnumExtensions
    {
        public static int AsInt(this RoleEnum status)
        {
            return (int)status;
        }
    }
}
