using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using UniPro.Domain.Entities.SuperTokens;

namespace UniPro.Domain.Entities;

[PrimaryKey("UserId")]
public class User
{
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("user_id")]
    [StringLength(36)]
    public string UserId { get; set; } = null!;
    
    [StringLength(255)]
    public string FirstName { get; set; } = null!;
    
    [StringLength(255)]
    public string LastName { get; set; } = null!;
    
    [StringLength(255)]
    public string Patronymic { get; set; } = null!;
    
    [StringLength(1024)]
    public string? Avatar { get; set; }
    
    [StringLength(36)]
    public string PhoneNumber { get; set; } = null!;
    
    public DateTime? CreatedAt { get; set; }
    
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("AppId, UserId")]
    [InverseProperty("Users")]
    public StAppIdToUserId StAppIdToUserId { get; set; } = null!;
    
    [InverseProperty("Student")]
    public StudentInfo StudentInfo { get; set; } = null!;
    
    [InverseProperty("Teacher")]
    public TeacherInfo TeacherInfo { get; set; } = null!;
}