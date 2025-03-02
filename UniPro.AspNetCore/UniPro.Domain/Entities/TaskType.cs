using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities;

[PrimaryKey("TaskTypeId")]
public class TaskType
{
    [Key]
    public int TaskTypeId { get; set; }
    
    public TaskTypeEnum Type { get; set; }
    
    [InverseProperty("TaskType")]
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}