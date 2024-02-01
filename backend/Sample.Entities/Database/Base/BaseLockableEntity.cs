using System.ComponentModel.DataAnnotations.Schema;

namespace BE.LocalAccountabilitySystem.Entities.Database.Base
{
    public class BaseLockableEntity : BaseEntity
    {
        [ForeignKey("LockedByUser")]
        public int? LockedByUserId { get; set; }

        public User LockedByUser { get; set; }
    }
}
