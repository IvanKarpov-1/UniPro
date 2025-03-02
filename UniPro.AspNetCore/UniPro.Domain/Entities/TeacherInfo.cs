using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities;

[PrimaryKey("AppId", "TeacherId")]
public class TeacherInfo
{
    [Key]
    [StringLength(64)]
    public string AppId { get; set; } = null!;
    
    [Key]
    [StringLength(36)]
    public string TeacherId { get; set; } = null!;
    
    public int DepartmentId { get; set; }
    
    public int AcademicId { get; set; }
    
    public int UniversityId { get; set; }

    [ForeignKey("AppId, TeacherId")]
    [InverseProperty("TeacherInfo")]
    public User Teacher { get; set; } = null!;

    [ForeignKey("DepartmentId")]
    [InverseProperty("TeacherInfos")]
    public Department Department { get; set; } = null!;

    [ForeignKey("AcademicId")]
    [InverseProperty("TeacherInfos")]
    public Academic Academic { get; set; } = null!;

    [ForeignKey("UniversityId")]
    [InverseProperty("TeacherInfos")]
    public University University { get; set; } = null!;
    
    [InverseProperty("Teacher")]
    public ICollection<Task> Tasks { get; set; } = null!;
}