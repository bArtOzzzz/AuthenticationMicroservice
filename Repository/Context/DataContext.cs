using Microsoft.EntityFrameworkCore;
using Repositories.Entities;

namespace Repositories.Context
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            Database.EnsureCreated();
        }

        public DbSet<UserEntity> Users { get; set; } = default!;
        public DbSet<RoleEntity> Roles { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Guid[] GuidRoleArr = { Guid.NewGuid(), Guid.NewGuid() };

            modelBuilder.Entity<UserEntity>(
                entity =>
                {
                    entity.Property(e => e.Id)
                          .IsRequired();

                    entity.HasOne(u => u.Role)
                          .WithMany(r => r.Users)
                          .HasForeignKey(u => u.RoleId);

                    entity.HasIndex(n => n.Username)
                          .IsUnique();
                });

            modelBuilder.Entity<RoleEntity>(
                entity =>
                {
                    entity.Property(e => e.Id)
                          .IsRequired();
                });

            // SEEDDATA
            modelBuilder.Entity<UserEntity>().HasData(
                new UserEntity
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    EmailAddress = "SonicHedgehog@gmail.com",
                    Username = "Sonic",
                    RoleId = GuidRoleArr[0],
                    Password = BCrypt.Net.BCrypt.HashPassword("administrator")
                },

                new UserEntity
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    EmailAddress = "User@gmail.com",
                    Username = "User",
                    RoleId = GuidRoleArr[1],
                    Password = BCrypt.Net.BCrypt.HashPassword("useruser")
                });

            modelBuilder.Entity<RoleEntity>().HasData(
                new RoleEntity
                {
                    Id = GuidRoleArr[0],
                    CreatedDate = DateTime.UtcNow,
                    Role = "Administrator"
                },

                new RoleEntity
                {
                    Id = GuidRoleArr[1],
                    CreatedDate = DateTime.UtcNow,
                    Role = "User"
                });
        }
    }
}
