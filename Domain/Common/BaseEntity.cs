using System.ComponentModel.DataAnnotations;

namespace Domain.Common;

public abstract class BaseEntity 
{
    public int Id { get; set; }
    public virtual string GetEntityName()
    {
        return GetType().Name;
    }
    [Required]
    public bool IsDeleted { get; set; } = false;
}