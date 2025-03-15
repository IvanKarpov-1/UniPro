using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities;

[PrimaryKey("CourseId")]
public class Course
{
    [Key]
    public long CourseId { get; set; }
    
    [StringLength(128)]
    public string CourseName { get; set; } = null!;
    
    public int Credits { get; set; }
    
    [InverseProperty("Course")]
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
}