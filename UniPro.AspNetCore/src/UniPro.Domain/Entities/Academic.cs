using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities;

[PrimaryKey("AcademicId")]
public class Academic
{
    [Key]
    public int AcademicId { get; set; }
    
    public int UniversityId { get; set; }
    
    [StringLength(512)]
    public string Name { get; set; } = null!;
    
    [InverseProperty("Academic")]
    public ICollection<StudentInfo> StudentInfos { get; set; } = new List<StudentInfo>();
    
    [InverseProperty("Academic")]
    public ICollection<TeacherInfo> TeacherInfos { get; set; } = new List<TeacherInfo>();

    [ForeignKey("UniversityId")]
    [InverseProperty("Academics")]
    public University University { get; set; } = null!;
    
    [InverseProperty("Academic")]
    public ICollection<Department> Departments { get; set; } = new List<Department>();
}