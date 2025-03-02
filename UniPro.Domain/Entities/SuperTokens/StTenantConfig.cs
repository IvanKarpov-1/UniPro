using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("ConnectionUriDomain", "AppId", "TenantId")]
[Table("st_tenant_configs")]
public partial class StTenantConfig
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

    [Column("core_config")]
    public string? CoreConfig { get; set; }

    [Column("email_password_enabled")]
    public bool? EmailPasswordEnabled { get; set; }

    [Column("passwordless_enabled")]
    public bool? PasswordlessEnabled { get; set; }

    [Column("third_party_enabled")]
    public bool? ThirdPartyEnabled { get; set; }

    [Column("is_first_factors_null")]
    public bool? IsFirstFactorsNull { get; set; }

    [InverseProperty("StTenantConfig")]
    public virtual ICollection<StTenantFirstFactor> StTenantFirstFactors { get; set; } = new List<StTenantFirstFactor>();

    [InverseProperty("StTenantConfig")]
    public virtual ICollection<StTenantRequiredSecondaryFactor> StTenantRequiredSecondaryFactors { get; set; } = new List<StTenantRequiredSecondaryFactor>();

    [InverseProperty("StTenantConfig")]
    public virtual ICollection<StTenantThirdpartyProvider> StTenantThirdpartyProviders { get; set; } = new List<StTenantThirdpartyProvider>();
}
