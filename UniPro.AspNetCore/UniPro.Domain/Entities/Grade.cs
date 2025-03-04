using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities;

[PrimaryKey("StudentId", "TaskId")]
public class Grade
{
    [Key]
    [StringLength(36)]
    public string StudentId { get; set; } = null!;
    
    [Key]
    public long TaskId { get; set; }
    
    public float GradeValue { get; set; }
    
    public bool IsAgreed { get; set; }
    
    [ForeignKey("StudentId, TaskId")]
    [InverseProperty("Grade")]
    public StudentTask StudentTask { get; set; } = null!;
}