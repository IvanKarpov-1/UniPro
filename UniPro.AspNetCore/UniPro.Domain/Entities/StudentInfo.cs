using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities;

[PrimaryKey("AppId", "StudentId")]
public class StudentInfo
{
    [Key]
    [StringLength(64)]
    public string AppId { get; set; } = null!;
    
    [Key]
    [StringLength(36)]
    public string StudentId { get; set; } = null!;
    
    public int StudentGroupId { get; set; }
    
    public int DepartmentId { get; set; }
    
    public int AcademicId { get; set; }
    
    public int UniversityId { get; set; }

    [ForeignKey("AppId, StudentId")]
    [InverseProperty("StudentInfo")]
    public User Student { get; set; } = null!;

    [ForeignKey("StudentGroupId")]
    [InverseProperty("StudentInfos")]
    public StudentGroup StudentGroup { get; set; } = null!;

    [ForeignKey("DepartmentId")]
    [InverseProperty("StudentInfos")]
    public Department Department { get; set; } = null!;

    [ForeignKey("AcademicId")]
    [InverseProperty("StudentInfos")]
    public Academic Academic { get; set; } = null!;

    [ForeignKey("UniversityId")]
    [InverseProperty("StudentInfos")]
    public University University { get; set; } = null!;
    
    [InverseProperty("Student")]
    public ICollection<StudentTask> StudentTasks { get; set; } = null!;
}