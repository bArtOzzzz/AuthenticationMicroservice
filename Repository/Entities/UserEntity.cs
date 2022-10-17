using Repositories.Entities.Abstract;

namespace Repositories.Entities
{
    public class UserEntity : BaseEntity
    {
        public Guid RoleId { get; set; }
        public string? Username { get; set; }
        public string? EmailAddress { get; set; }
        public string? Password { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public RoleEntity? Role { get; set; }
    }
}
