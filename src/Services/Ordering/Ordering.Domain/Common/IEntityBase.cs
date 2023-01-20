namespace Ordering.Domain.Common;

public interface IEntityBase
{
    string CreatedBy { get; set; }
    DateTime CreatedAt { get; set; }
    string? LastModifiedBy { get; set; }
    DateTime? LastModifiedAt { get; set; }
}
