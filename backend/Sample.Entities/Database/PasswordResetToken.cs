using BE.LocalAccountabilitySystem.Entities.Database.Base;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BE.LocalAccountabilitySystem.Entities.Database
{
    public class PasswordResetToken : BaseEntity
    {
        [ForeignKey("User")]
        [Required]
        public int UserId { get; set; }

        [Required]
        public byte[] Token { get; set; }

        [Required]
        public bool IsUsed { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; }

        public User User { get; set; }

    }
}
