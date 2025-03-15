using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "UserId")]
[Table("st_dashboard_users")]
[Index("AppId", Name = "dashboard_users_app_id_index")]
[Index("AppId", "Email", Name = "st_dashboard_users_email_key", IsUnique = true)]
public partial class StDashboardUser
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

    [ForeignKey("AppId")]
    [InverseProperty("StDashboardUsers")]
    public virtual StApp App { get; set; } = null!;

    [InverseProperty("StDashboardUser")]
    public virtual ICollection<StDashboardUserSession> StDashboardUserSessions { get; set; } = new List<StDashboardUserSession>();
}
