using System.ComponentModel.DataAnnotations;

namespace Domain.Common;

public abstract class BaseEntity 
{
    public int Id { get; set; }
    [Required]
    public bool IsDeleted { get; set; } = false;
}