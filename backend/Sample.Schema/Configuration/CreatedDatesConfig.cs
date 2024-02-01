using BE.LocalAccountabilitySystem.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace BE.LocalAccountabilitySystem.Schema
{
    /// <summary>
    /// A static class that uses <see cref="ModelBuilder"/> to configure and set information for
    /// the CreatedDate fields
    /// </summary>
    public static class CreatedDatesConfig
    {
        /// <summary>
        /// Configure all entities CreatedDate to be defaulted to GETUTCDATE()
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void ConfigureCreatedDates(this ModelBuilder modelBuilder)
        {
            #region Lookups

            modelBuilder.Entity<Role>()
                        .Property(b => b.CreatedDate)
                        .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<SystemStatus>()
                        .Property(b => b.CreatedDate)
                        .HasDefaultValueSql("GETUTCDATE()");

            #endregion

            #region Tables

            
            modelBuilder.Entity<PasswordResetToken>()
                        .Property(b => b.CreatedDate)
                        .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<User>()
                        .Property(b => b.CreatedDate)
                        .HasDefaultValueSql("GETUTCDATE()");

            modelBuilder.Entity<UserToRole>()
                        .Property(b => b.CreatedDate)
                        .HasDefaultValueSql("GETUTCDATE()");

            #endregion
        }
    }
}
