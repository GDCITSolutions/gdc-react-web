using BE.LocalAccountabilitySystem.Entities.Database;
using BE.LocalAccountabilitySystem.Schema.Configuration;
using Microsoft.EntityFrameworkCore;

namespace BE.LocalAccountabilitySystem.Schema
{
    public class SampleContext : DbContext
    {
        public SampleContext(DbContextOptions<SampleContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // turn off cascade delete behavior
            // revisit if we end up needing cascading deletes
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
                relationship.DeleteBehavior = DeleteBehavior.Restrict;

            // entities should have a default created date while modified dates are set by a trigger
            CreatedDatesConfig.ConfigureCreatedDates(modelBuilder);
            TriggerConfig.ConfigureTiggers(modelBuilder);

            SeedDataConfig.Seed(modelBuilder);

            modelBuilder.Entity<PasswordResetToken>()
                .HasOne(prt => prt.User)
                .WithMany(u => u.PasswordResetTokens)
                .HasForeignKey(prt => prt.UserId);

            modelBuilder.Entity<UserToRole>()
                .HasOne(utr => utr.User)
                .WithMany(u => u.Roles)
                .HasForeignKey(utr => utr.UserId);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        #region Tables
        public DbSet<User> User { get; set; }
        public DbSet<UserToRole> UserToRole { get; set; }
        public DbSet<PasswordResetToken> PasswordResetToken { get; set; }

        #endregion

        #region Lookups

        public DbSet<Role> Role { get; set; }
        public DbSet<SystemStatus> SystemStatus { get; set; }

        #endregion
    }
}
