namespace Ordering.Domain.Common;

public abstract class EntityBase<TKey> : IEntityBase
{
    public TKey Id { get; protected set; }
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedAt { get; set; }
}
