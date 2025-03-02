using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "UserId")]
[Table("st_totp_users")]
[Index("AppId", Name = "totp_users_app_id_index")]
public partial class StTotpUser
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("user_id")]
    [StringLength(128)]
    public string UserId { get; set; } = null!;

    [ForeignKey("AppId")]
    [InverseProperty("StTotpUsers")]
    public virtual StApp App { get; set; } = null!;

    [InverseProperty("StTotpUser")]
    public virtual ICollection<StTotpUsedCode> StTotpUsedCodes { get; set; } = new List<StTotpUsedCode>();

    [InverseProperty("StTotpUser")]
    public virtual ICollection<StTotpUserDevice> StTotpUserDevices { get; set; } = new List<StTotpUserDevice>();
}
