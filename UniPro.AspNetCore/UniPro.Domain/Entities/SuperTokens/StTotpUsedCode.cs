using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "TenantId", "UserId", "CreatedTimeMs")]
[Table("st_totp_used_codes")]
[Index("AppId", "TenantId", "ExpiryTimeMs", Name = "totp_used_codes_expiry_time_ms_index")]
[Index("AppId", "TenantId", Name = "totp_used_codes_tenant_id_index")]
[Index("AppId", "UserId", Name = "totp_used_codes_user_id_index")]
public partial class StTotpUsedCode
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
    [Column("user_id")]
    [StringLength(128)]
    public string UserId { get; set; } = null!;

    [Column("code")]
    [StringLength(8)]
    public string Code { get; set; } = null!;

    [Column("is_valid")]
    public bool IsValid { get; set; }

    [Column("expiry_time_ms")]
    public long ExpiryTimeMs { get; set; }

    [Key]
    [Column("created_time_ms")]
    public long CreatedTimeMs { get; set; }

    [ForeignKey("AppId, TenantId")]
    [InverseProperty("StTotpUsedCodes")]
    public virtual StTenant StTenant { get; set; } = null!;

    [ForeignKey("AppId, UserId")]
    [InverseProperty("StTotpUsedCodes")]
    public virtual StTotpUser StTotpUser { get; set; } = null!;
}
