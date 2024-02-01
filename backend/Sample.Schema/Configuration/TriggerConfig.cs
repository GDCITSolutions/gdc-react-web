using BE.LocalAccountabilitySystem.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace BE.LocalAccountabilitySystem.Schema
{
    public static class TriggerConfig
    {
        /// <summary>
        /// Specify triggers that are present on tables
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void ConfigureTiggers(this ModelBuilder modelBuilder)
        {
            #region Lookups

            modelBuilder.Entity<SystemStatus>(entry =>
            {
                entry.ToTable("SystemStatus", tb => tb.HasTrigger("SystemStatusUpdateTrigger"));
            });

            #endregion

            #region Tables

            modelBuilder.Entity<PasswordResetToken>(entry =>
            {
                entry.ToTable("PasswordResetToken", tb => tb.HasTrigger("PasswordResetTokenUpdateTrigger"));
            });

            modelBuilder.Entity<User>(entry =>
            {
                entry.ToTable("User", tb => tb.HasTrigger("UserUpdateTrigger"));
            });

            modelBuilder.Entity<UserToRole>(entry =>
            {
                entry.ToTable("UserToRole", tb => tb.HasTrigger("UserToRoleUpdateTrigger"));
            });

            #endregion
        }
    }
}
