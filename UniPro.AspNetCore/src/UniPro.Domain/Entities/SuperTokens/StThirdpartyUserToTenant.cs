using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "TenantId", "UserId")]
[Table("st_thirdparty_user_to_tenant")]
[Index("AppId", "TenantId", "ThirdPartyId", "ThirdPartyUserId", Name = "st_thirdparty_user_to_tenant_third_party_user_id_key", IsUnique = true)]
[Index("AppId", "TenantId", "ThirdPartyId", "ThirdPartyUserId", Name = "thirdparty_user_to_tenant_third_party_user_id_index")]
public partial class StThirdpartyUserToTenant
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
    [StringLength(36)]
    public string UserId { get; set; } = null!;

    [Column("third_party_id")]
    [StringLength(28)]
    public string ThirdPartyId { get; set; } = null!;

    [Column("third_party_user_id")]
    [StringLength(256)]
    public string ThirdPartyUserId { get; set; } = null!;

    [ForeignKey("AppId, TenantId, UserId")]
    [InverseProperty("StThirdpartyUserToTenant")]
    public virtual StAllAuthRecipeUser StAllAuthRecipeUser { get; set; } = null!;
}
