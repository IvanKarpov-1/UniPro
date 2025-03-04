using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities;

[PrimaryKey("StudentId", "TaskId")]
public class StudentTask
{
    [Key]
    [StringLength(36)]
    public string StudentId { get; set; } = null!;
    
    [Key]
    public long TaskId { get; set; }
    
    [ForeignKey("StudentId")]
    [InverseProperty("StudentTasks")]
    public StudentInfo Student { get; set; } = null!;
    
    [ForeignKey("TaskTypeId")]
    [InverseProperty("StudentTasks")]
    public Task Task { get; set; } = null!;
    
    [InverseProperty("StudentTask")]
    public Grade Grade { get; set; } = null!;
}