using Repositories.Entities.Abstract;

namespace Repositories.Entities
{
    public class RoleEntity : BaseEntity
    {
        public string Role { get; set; }
        public List<UserEntity> Users { get; set; }
    }
}
