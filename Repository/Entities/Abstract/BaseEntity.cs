namespace Repositories.Entities.Abstract
{
    public abstract class BaseEntity : IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
