using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "UserId")]
[Table("st_passwordless_users")]
[Index("AppId", "Email", Name = "passwordless_users_email_index")]
[Index("AppId", "PhoneNumber", Name = "passwordless_users_phone_number_index")]
public partial class StPasswordlessUser
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
    public string? Email { get; set; }

    [Column("phone_number")]
    [StringLength(256)]
    public string? PhoneNumber { get; set; }

    [Column("time_joined")]
    public long TimeJoined { get; set; }

    [ForeignKey("AppId, UserId")]
    [InverseProperty("StPasswordlessUser")]
    public virtual StAppIdToUserId StAppIdToUserId { get; set; } = null!;
}
