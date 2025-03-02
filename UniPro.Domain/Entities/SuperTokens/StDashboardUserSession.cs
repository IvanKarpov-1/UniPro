using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "SessionId")]
[Table("st_dashboard_user_sessions")]
[Index("Expiry", Name = "dashboard_user_sessions_expiry_index")]
[Index("AppId", "UserId", Name = "dashboard_user_sessions_user_id_index")]
public partial class StDashboardUserSession
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("session_id")]
    [StringLength(36)]
    public string SessionId { get; set; } = null!;

    [Column("user_id")]
    [StringLength(36)]
    public string UserId { get; set; } = null!;

    [Column("time_created")]
    public long TimeCreated { get; set; }

    [Column("expiry")]
    public long Expiry { get; set; }

    [ForeignKey("AppId, UserId")]
    [InverseProperty("StDashboardUserSessions")]
    public virtual StDashboardUser StDashboardUser { get; set; } = null!;
}
