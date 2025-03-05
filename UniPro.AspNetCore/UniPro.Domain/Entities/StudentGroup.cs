using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities;

[PrimaryKey("StudentGroupId")]
public class StudentGroup
{
    [Key]
    public int StudentGroupId { get; set; }
    
    public int DepartmentId { get; set; }
    
    [StringLength(32)]
    public string Name { get; set; } = null!;
    
    [InverseProperty("StudentGroup")]
    public ICollection<StudentInfo> StudentInfos { get; set; } = new List<StudentInfo>();

    [ForeignKey("DepartmentId")]
    [InverseProperty("StudentGroups")]
    public Department Department { get; set; } = null!;
}