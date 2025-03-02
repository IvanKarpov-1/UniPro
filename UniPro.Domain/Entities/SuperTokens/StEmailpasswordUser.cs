using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "UserId")]
[Table("st_emailpassword_users")]
[Index("AppId", "Email", Name = "emailpassword_users_email_index")]
public partial class StEmailpasswordUser
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("user_id")]
    [StringLength(36)]
    public string UserId { get; set; } = null!;

    [Column("email")]
    [StringLength(256)]
    public string Email { get; set; } = null!;

    [Column("password_hash")]
    [StringLength(256)]
    public string PasswordHash { get; set; } = null!;

    [Column("time_joined")]
    public long TimeJoined { get; set; }

    [ForeignKey("AppId, UserId")]
    [InverseProperty("StEmailpasswordUser")]
    public virtual StAppIdToUserId StAppIdToUserId { get; set; } = null!;
}
