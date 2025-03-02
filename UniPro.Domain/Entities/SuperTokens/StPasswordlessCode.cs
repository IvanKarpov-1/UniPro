using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "TenantId", "CodeId")]
[Table("st_passwordless_codes")]
[Index("AppId", "TenantId", "CreatedAt", Name = "passwordless_codes_created_at_index")]
[Index("AppId", "TenantId", "DeviceIdHash", Name = "passwordless_codes_device_id_hash_index")]
[Index("AppId", "TenantId", "LinkCodeHash", Name = "st_passwordless_codes_link_code_hash_key", IsUnique = true)]
public partial class StPasswordlessCode
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
    [Column("code_id")]
    [StringLength(36)]
    public string CodeId { get; set; } = null!;

    [Column("device_id_hash")]
    [StringLength(44)]
    public string DeviceIdHash { get; set; } = null!;

    [Column("link_code_hash")]
    [StringLength(44)]
    public string LinkCodeHash { get; set; } = null!;

    [Column("created_at")]
    public long CreatedAt { get; set; }

    [ForeignKey("AppId, TenantId, DeviceIdHash")]
    [InverseProperty("StPasswordlessCodes")]
    public virtual StPasswordlessDevice StPasswordlessDevice { get; set; } = null!;
}
