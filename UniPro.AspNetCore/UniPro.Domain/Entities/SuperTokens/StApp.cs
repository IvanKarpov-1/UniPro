using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniPro.Domain.Entities.SuperTokens;

[Table("st_apps")]
public partial class StApp
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Column("created_at_time")]
    public long? CreatedAtTime { get; set; }

    [InverseProperty("App")]
    public virtual ICollection<StAppIdToUserId> StAppIdToUserIds { get; set; } = new List<StAppIdToUserId>();

    [InverseProperty("App")]
    public virtual ICollection<StBulkImportUser> StBulkImportUsers { get; set; } = new List<StBulkImportUser>();

    [InverseProperty("App")]
    public virtual ICollection<StDashboardUser> StDashboardUsers { get; set; } = new List<StDashboardUser>();

    [InverseProperty("App")]
    public virtual ICollection<StEmailverificationVerifiedEmail> StEmailverificationVerifiedEmails { get; set; } = new List<StEmailverificationVerifiedEmail>();

    [InverseProperty("App")]
    public virtual ICollection<StJwtSigningKey> StJwtSigningKeys { get; set; } = new List<StJwtSigningKey>();

    [InverseProperty("App")]
    public virtual ICollection<StOauthClient> StOauthClients { get; set; } = new List<StOauthClient>();

    [InverseProperty("App")]
    public virtual ICollection<StRole> StRoles { get; set; } = new List<StRole>();

    [InverseProperty("App")]
    public virtual ICollection<StSessionAccessTokenSigningKey> StSessionAccessTokenSigningKeys { get; set; } = new List<StSessionAccessTokenSigningKey>();

    [InverseProperty("App")]
    public virtual ICollection<StTenant> StTenants { get; set; } = new List<StTenant>();

    [InverseProperty("App")]
    public virtual ICollection<StTotpUser> StTotpUsers { get; set; } = new List<StTotpUser>();

    [InverseProperty("App")]
    public virtual ICollection<StUserLastActive> StUserLastActives { get; set; } = new List<StUserLastActive>();

    [InverseProperty("App")]
    public virtual ICollection<StUserMetadatum> StUserMetadata { get; set; } = new List<StUserMetadatum>();
    
}
