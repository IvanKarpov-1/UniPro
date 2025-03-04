using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities;

[PrimaryKey("TaskId")]
public class Task
{
    [Key]
    public long TaskId { get; set; }
    
    public long CourseId { get; set; }
    
    public int TaskTypeId { get; set; }
    
    [StringLength(36)]
    public string TeacherId { get; set; } = null!;
        
    [StringLength(128)]
    public string Name { get; set; } = null!;
    
    public DateTime? DueDate { get; set; }
    
    [ForeignKey("CourseId")]
    [InverseProperty("Tasks")]
    public Course Course { get; set; } = null!;
    
    [ForeignKey("TaskTypeId")]
    [InverseProperty("Tasks")]
    public TaskType TaskType { get; set; } = null!;

    [ForeignKey("TeacherId")]
    [InverseProperty("Tasks")]
    public TeacherInfo Teacher { get; set; } = null!;
    
    [InverseProperty("Task")]
    public ICollection<StudentTask> StudentTasks { get; set; } = null!;
}