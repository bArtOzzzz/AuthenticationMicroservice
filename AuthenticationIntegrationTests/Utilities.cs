using Repositories.Entities;
using Repositories.Context;

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
                    Id = new Guid("cefb4981-6f36-4405-b633-361270085433"),
                    CreatedDate = DateTime.UtcNow,
                    EmailAddress = "SonicHedgehog@gmail.com",
                    Username = "Sonic",
                    RoleId = GetRole()[0].Id,
                    Password = BCrypt.Net.BCrypt.HashPassword("administrator")
                },

                new UserEntity
                {
                    Id = new Guid("0da216b5-873d-491f-9641-a6b9ebaf7ee3"),
                    CreatedDate = DateTime.UtcNow,
                    EmailAddress = "User@gmail.com",
                    Username = "User",
                    RoleId = GetRole()[1].Id,
                    Password = BCrypt.Net.BCrypt.HashPassword("useruser")
                },

                new UserEntity
                {
                    Id = new Guid("071fc8e4-28b9-4bf4-b169-9ef3a063ce8b"),
                    CreatedDate = DateTime.UtcNow,
                    EmailAddress = "testuser@gmail.com",
                    Username = "TestUser",
                    RoleId = GetRole()[0].Id,
                    Password = BCrypt.Net.BCrypt.HashPassword("administrator")
                },

                new UserEntity
                {
                    Id = new Guid("94749344-4583-406e-9e76-846c39e6b6d2"),
                    CreatedDate = DateTime.UtcNow,
                    EmailAddress = "testuser@gmail.com",
                    Username = "AnotherTestUserName",
                    RoleId = GetRole()[0].Id,
                    Password = BCrypt.Net.BCrypt.HashPassword("administrator")
                }
            };
        }

        public static List<RoleEntity> GetRole()
        {
            return new List<RoleEntity>()
            {
                new RoleEntity
                {
                    Id = new Guid("a446525a-41c8-4722-8152-5c72e3efd01d"),
                    CreatedDate = DateTime.UtcNow,
                    Role = "Administrator"
                },

                new RoleEntity
                {
                    Id = new Guid("01ff0f71-519b-48f6-8b78-3e1210d2495b"),
                    CreatedDate = DateTime.UtcNow,
                    Role = "User"
                }
            };
        }
    }
}
