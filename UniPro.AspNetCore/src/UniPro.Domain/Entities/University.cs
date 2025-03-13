using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities;

[PrimaryKey("UniversityId")]
public class University
{
    [Key]
    public int UniversityId { get; set; }
    
    [StringLength(256)]
    public string Name { get; set; } = null!;
    
    [InverseProperty("University")]
    public ICollection<StudentInfo> StudentInfos { get; set; } = new List<StudentInfo>();
    
    [InverseProperty("University")]
    public ICollection<TeacherInfo> TeacherInfos { get; set; } = new List<TeacherInfo>();
    
    [InverseProperty("University")]
    public ICollection<Academic> Academics { get; set; } = new List<Academic>();
}