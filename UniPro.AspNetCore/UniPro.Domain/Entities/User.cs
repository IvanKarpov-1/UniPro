using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using UniPro.Domain.Entities.SuperTokens;

namespace UniPro.Domain.Entities;

[PrimaryKey("AppId", "UserId")]
public class User
{
    [Key]
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
    public string Avatar { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    
    [ForeignKey("AppId, UserId")]
    [InverseProperty("Users")]
    public required StAppIdToUserId StAppIdToUserId { get; set; }
}