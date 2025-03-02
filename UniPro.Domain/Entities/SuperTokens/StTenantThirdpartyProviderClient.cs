using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("ConnectionUriDomain", "AppId", "TenantId", "ThirdPartyId", "ClientType")]
[Table("st_tenant_thirdparty_provider_clients")]
[Index("ConnectionUriDomain", "AppId", "TenantId", "ThirdPartyId", Name = "tenant_thirdparty_provider_clients_third_party_id_index")]
public partial class StTenantThirdpartyProviderClient
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
    [Column("third_party_id")]
    [StringLength(28)]
    public string ThirdPartyId { get; set; } = null!;

    [Key]
    [Column("client_type")]
    [StringLength(64)]
    public string ClientType { get; set; } = null!;

    [Column("client_id")]
    [StringLength(256)]
    public string ClientId { get; set; } = null!;

    [Column("client_secret")]
    public string? ClientSecret { get; set; }

    [Column("scope", TypeName = "character varying(128)[]")]
    public List<string>? Scope { get; set; }

    [Column("force_pkce")]
    public bool? ForcePkce { get; set; }

    [Column("additional_config")]
    public string? AdditionalConfig { get; set; }

    [ForeignKey("ConnectionUriDomain, AppId, TenantId, ThirdPartyId")]
    [InverseProperty("StTenantThirdpartyProviderClients")]
    public virtual StTenantThirdpartyProvider StTenantThirdpartyProvider { get; set; } = null!;
}
