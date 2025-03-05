using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities;

[PrimaryKey("DepartmentId")]
public class Department
{
    [Key]
    public int DepartmentId { get; set; }
    
    public int AcademicId { get; set; }
    
    [StringLength(512)]
    public string Name { get; set; } = null!;
    
    [InverseProperty("Department")]
    public ICollection<StudentInfo> StudentInfos { get; set; } = new List<StudentInfo>();
    
    [InverseProperty("Department")]
    public ICollection<TeacherInfo> TeacherInfos { get; set; } = new List<TeacherInfo>();

    [ForeignKey("AcademicId")]
    [InverseProperty("Departments")]
    public Academic Academic { get; set; } = null!;
    
    [InverseProperty("Department")]
    public ICollection<StudentGroup> StudentGroups { get; set; } = new List<StudentGroup>();
}