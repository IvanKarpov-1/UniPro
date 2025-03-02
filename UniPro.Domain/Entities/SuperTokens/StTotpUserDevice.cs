using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "UserId", "DeviceName")]
[Table("st_totp_user_devices")]
[Index("AppId", "UserId", Name = "totp_user_devices_user_id_index")]
public partial class StTotpUserDevice
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("user_id")]
    [StringLength(128)]
    public string UserId { get; set; } = null!;

    [Key]
    [Column("device_name")]
    [StringLength(256)]
    public string DeviceName { get; set; } = null!;

    [Column("secret_key")]
    [StringLength(256)]
    public string SecretKey { get; set; } = null!;

    [Column("period")]
    public int Period { get; set; }

    [Column("skew")]
    public int Skew { get; set; }

    [Column("verified")]
    public bool Verified { get; set; }

    [Column("created_at")]
    public long? CreatedAt { get; set; }

    [ForeignKey("AppId, UserId")]
    [InverseProperty("StTotpUserDevices")]
    public virtual StTotpUser StTotpUser { get; set; } = null!;
}
