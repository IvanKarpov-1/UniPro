using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "TenantId", "DeviceIdHash")]
[Table("st_passwordless_devices")]
[Index("AppId", "TenantId", "Email", Name = "passwordless_devices_email_index")]
[Index("AppId", "TenantId", "PhoneNumber", Name = "passwordless_devices_phone_number_index")]
[Index("AppId", "TenantId", Name = "passwordless_devices_tenant_id_index")]
public partial class StPasswordlessDevice
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
    [Column("device_id_hash")]
    [StringLength(44)]
    public string DeviceIdHash { get; set; } = null!;

    [Column("email")]
    [StringLength(256)]
    public string? Email { get; set; }

    [Column("phone_number")]
    [StringLength(256)]
    public string? PhoneNumber { get; set; }

    [Column("link_code_salt")]
    [StringLength(44)]
    public string LinkCodeSalt { get; set; } = null!;

    [Column("failed_attempts")]
    public int FailedAttempts { get; set; }

    [InverseProperty("StPasswordlessDevice")]
    public virtual ICollection<StPasswordlessCode> StPasswordlessCodes { get; set; } = new List<StPasswordlessCode>();

    [ForeignKey("AppId, TenantId")]
    [InverseProperty("StPasswordlessDevices")]
    public virtual StTenant StTenant { get; set; } = null!;
}
