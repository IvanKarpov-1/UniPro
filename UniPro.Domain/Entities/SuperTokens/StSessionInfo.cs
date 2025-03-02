using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "TenantId", "SessionHandle")]
[Table("st_session_info")]
[Index("ExpiresAt", Name = "session_expiry_index")]
[Index("AppId", "TenantId", Name = "session_info_tenant_id_index")]
[Index("UserId", "AppId", Name = "session_info_user_id_app_id_index")]
public partial class StSessionInfo
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("tenant_id")]
    [StringLength(64)]
    public string TenantId { get; set; } = null!;

    [Key]
    [Column("session_handle")]
    [StringLength(255)]
    public string SessionHandle { get; set; } = null!;

    [Column("user_id")]
    [StringLength(128)]
    public string UserId { get; set; } = null!;

    [Column("refresh_token_hash_2")]
    [StringLength(128)]
    public string RefreshTokenHash2 { get; set; } = null!;

    [Column("session_data")]
    public string? SessionData { get; set; }

    [Column("expires_at")]
    public long ExpiresAt { get; set; }

    [Column("created_at_time")]
    public long CreatedAtTime { get; set; }

    [Column("jwt_user_payload")]
    public string? JwtUserPayload { get; set; }

    [Column("use_static_key")]
    public bool UseStaticKey { get; set; }

    [ForeignKey("AppId, TenantId")]
    [InverseProperty("StSessionInfos")]
    public virtual StTenant StTenant { get; set; } = null!;
}
