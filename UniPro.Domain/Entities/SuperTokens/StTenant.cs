using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "TenantId")]
[Table("st_tenants")]
[Index("AppId", Name = "tenants_app_id_index")]
public partial class StTenant
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("tenant_id")]
    [StringLength(64)]
    public string TenantId { get; set; } = null!;

    [Column("created_at_time")]
    public long? CreatedAtTime { get; set; }

    [ForeignKey("AppId")]
    [InverseProperty("StTenants")]
    public virtual StApp App { get; set; } = null!;

    [InverseProperty("StTenant")]
    public virtual ICollection<StAllAuthRecipeUser> StAllAuthRecipeUsers { get; set; } = new List<StAllAuthRecipeUser>();

    [InverseProperty("StTenant")]
    public virtual ICollection<StEmailverificationToken> StEmailverificationTokens { get; set; } = new List<StEmailverificationToken>();

    [InverseProperty("StTenant")]
    public virtual ICollection<StKeyValue> StKeyValues { get; set; } = new List<StKeyValue>();

    [InverseProperty("StTenant")]
    public virtual ICollection<StPasswordlessDevice> StPasswordlessDevices { get; set; } = new List<StPasswordlessDevice>();

    [InverseProperty("StTenant")]
    public virtual ICollection<StSessionInfo> StSessionInfos { get; set; } = new List<StSessionInfo>();

    [InverseProperty("StTenant")]
    public virtual ICollection<StTotpUsedCode> StTotpUsedCodes { get; set; } = new List<StTotpUsedCode>();

    [InverseProperty("StTenant")]
    public virtual ICollection<StUserRole> StUserRoles { get; set; } = new List<StUserRole>();
}
