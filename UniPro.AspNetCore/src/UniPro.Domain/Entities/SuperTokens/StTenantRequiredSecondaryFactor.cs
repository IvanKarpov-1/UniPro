using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("ConnectionUriDomain", "AppId", "TenantId", "FactorId")]
[Table("st_tenant_required_secondary_factors")]
[Index("ConnectionUriDomain", "AppId", "TenantId", Name = "tenant_default_required_factor_ids_tenant_id_index")]
public partial class StTenantRequiredSecondaryFactor
{
    [Key]
    [Column("connection_uri_domain")]
    [StringLength(256)]
    public string ConnectionUriDomain { get; set; } = null!;

    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("tenant_id")]
    [StringLength(64)]
    public string TenantId { get; set; } = null!;

    [Key]
    [Column("factor_id")]
    [StringLength(128)]
    public string FactorId { get; set; } = null!;

    [ForeignKey("ConnectionUriDomain, AppId, TenantId")]
    [InverseProperty("StTenantRequiredSecondaryFactors")]
    public virtual StTenantConfig StTenantConfig { get; set; } = null!;
}
