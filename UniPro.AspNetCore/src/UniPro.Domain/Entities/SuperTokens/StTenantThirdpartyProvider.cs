using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("ConnectionUriDomain", "AppId", "TenantId", "ThirdPartyId")]
[Table("st_tenant_thirdparty_providers")]
[Index("ConnectionUriDomain", "AppId", "TenantId", Name = "tenant_thirdparty_providers_tenant_id_index")]
public partial class StTenantThirdpartyProvider
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

    [Column("name")]
    [StringLength(64)]
    public string? Name { get; set; }

    [Column("authorization_endpoint")]
    public string? AuthorizationEndpoint { get; set; }

    [Column("authorization_endpoint_query_params")]
    public string? AuthorizationEndpointQueryParams { get; set; }

    [Column("token_endpoint")]
    public string? TokenEndpoint { get; set; }

    [Column("token_endpoint_body_params")]
    public string? TokenEndpointBodyParams { get; set; }

    [Column("user_info_endpoint")]
    public string? UserInfoEndpoint { get; set; }

    [Column("user_info_endpoint_query_params")]
    public string? UserInfoEndpointQueryParams { get; set; }

    [Column("user_info_endpoint_headers")]
    public string? UserInfoEndpointHeaders { get; set; }

    [Column("jwks_uri")]
    public string? JwksUri { get; set; }

    [Column("oidc_discovery_endpoint")]
    public string? OidcDiscoveryEndpoint { get; set; }

    [Column("require_email")]
    public bool? RequireEmail { get; set; }

    [Column("user_info_map_from_id_token_payload_user_id")]
    [StringLength(64)]
    public string? UserInfoMapFromIdTokenPayloadUserId { get; set; }

    [Column("user_info_map_from_id_token_payload_email")]
    [StringLength(64)]
    public string? UserInfoMapFromIdTokenPayloadEmail { get; set; }

    [Column("user_info_map_from_id_token_payload_email_verified")]
    [StringLength(64)]
    public string? UserInfoMapFromIdTokenPayloadEmailVerified { get; set; }

    [Column("user_info_map_from_user_info_endpoint_user_id")]
    [StringLength(64)]
    public string? UserInfoMapFromUserInfoEndpointUserId { get; set; }

    [Column("user_info_map_from_user_info_endpoint_email")]
    [StringLength(64)]
    public string? UserInfoMapFromUserInfoEndpointEmail { get; set; }

    [Column("user_info_map_from_user_info_endpoint_email_verified")]
    [StringLength(64)]
    public string? UserInfoMapFromUserInfoEndpointEmailVerified { get; set; }

    [ForeignKey("ConnectionUriDomain, AppId, TenantId")]
    [InverseProperty("StTenantThirdpartyProviders")]
    public virtual StTenantConfig StTenantConfig { get; set; } = null!;

    [InverseProperty("StTenantThirdpartyProvider")]
    public virtual ICollection<StTenantThirdpartyProviderClient> StTenantThirdpartyProviderClients { get; set; } = new List<StTenantThirdpartyProviderClient>();
}
