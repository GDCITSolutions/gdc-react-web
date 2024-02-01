using BE.LocalAccountabilitySystem.Common.Util;
using BE.LocalAccountabilitySystem.Entities.Database.Base;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace BE.LocalAccountabilitySystem.Entities.Database
{
    [Index(nameof(EmailAddress), IsUnique = true)]
    public class User : BaseLockableEntity
    {
        #region Constructors

        public User() { }

        public User(int systemStatusId, string emailAddress)
        {
            Util.Guard.ArgumentsAreNotNull(systemStatusId, emailAddress);

            SystemStatusId = systemStatusId;
            EmailAddress = emailAddress;
        }

        #endregion

        #region Public Properties

        [StringLength(100)]
        public string FirstName { get; set; }

        [StringLength(100)]
        public string LastName { get; set; }

        [Required]
        [StringLength(200)]
        public string EmailAddress { get; set; }

        public byte[] Password { get; set; }

        public byte[] Salt { get; set; }

        #endregion

        #region Linked Entities

        public ICollection<PasswordResetToken> PasswordResetTokens { get; set; }
        public ICollection<UserToRole> Roles { get; set; }

        #endregion
    }
}
