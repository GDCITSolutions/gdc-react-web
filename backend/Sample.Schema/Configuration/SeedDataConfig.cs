using BE.LocalAccountabilitySystem.Entities.Database;
using Microsoft.EntityFrameworkCore;

namespace BE.LocalAccountabilitySystem.Schema.Configuration
{
    public static class SeedDataConfig
    {
        /// <summary>
        /// Seeds the specified tables with data.
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SystemStatus>().HasData(
                new SystemStatus() { Id = 1, Value = "Active", Description = "Active" },
                new SystemStatus() { Id = 2, Value = "Inactive", Description = "Inactive" },
                new SystemStatus() { Id = 3, Value = "Removed", Description = "Removed" },
                new SystemStatus() { Id = 4, Value = "Locked", Description = "Locked" });

            modelBuilder.Entity<Role>().HasData(
                new Role() { Id = 1, Value = "Level 1", Description = "Level 1", SystemStatusId = 1 },
                new Role() { Id = 2, Value = "Level 2", Description = "Level 2", SystemStatusId = 1 },
                new Role() { Id = 3, Value = "Level 3", Description = "Level 3", SystemStatusId = 1 },
                new Role() { Id = 4, Value = "Level 4", Description = "Level 4", SystemStatusId = 1 },
                new Role() { Id = 5, Value = "Level 5", Description = "Level 5", SystemStatusId = 1 });
        }
    }
}
