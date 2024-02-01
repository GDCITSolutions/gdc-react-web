using BE.LocalAccountabilitySystem.Entities.Database.Base;

namespace BE.LocalAccountabilitySystem.Entities.Database
{
    public class UserToRole : BaseEntity
    {
        public int UserId { get; set; }

        public int RoleId { get; set; }

        public User User { get; set; }

        public Role Role { get; set; }
    }
}
