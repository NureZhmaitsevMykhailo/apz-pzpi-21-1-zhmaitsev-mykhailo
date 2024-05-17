using System.ComponentModel.DataAnnotations;

namespace API.Core.Entities;

public abstract class BaseEntity
{
    [Key]
    public int Id { get; set; }
}
