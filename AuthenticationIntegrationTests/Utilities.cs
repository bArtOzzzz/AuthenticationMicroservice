using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Repositories.Context;
using Repositories.Entities;

namespace AuthenticationIntegrationTests
{
    static class Utilities
    {
        public static void InitializeDbForTests(DataContext db)
        {
            db.Roles.AddRange(GetRole());
            db.Users.AddRange(GetUsers());
            db.SaveChanges();
        }

        public static void ReinitializeDbForTests(DataContext db)
        {
            db.Roles.RemoveRange(db.Roles);
            db.Users.RemoveRange(db.Users);
            InitializeDbForTests(db);
        }

        public static List<UserEntity> GetUsers()
        {
            return new List<UserEntity>()
            {
                new UserEntity
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.UtcNow,
                    EmailAddress = "SonicHedgehog@gmail.com",
                    Username = "Sonic",
                    RoleId = GetRole().Id,
                    Password = BCrypt.Net.BCrypt.HashPassword("administrator")
                },
            };
        }

        public static RoleEntity GetRole()
        {
            return new RoleEntity
            {
                Id = new Guid("a446525a-41c8-4722-8152-5c72e3efd01d"),
                CreatedDate = DateTime.UtcNow,
                Role = "Administrator"
            };
        }
    }
}
