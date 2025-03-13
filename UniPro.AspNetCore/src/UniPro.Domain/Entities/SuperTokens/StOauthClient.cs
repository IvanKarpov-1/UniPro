using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "ClientId")]
[Table("st_oauth_clients")]
public partial class StOauthClient
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("client_id")]
    [StringLength(255)]
    public string ClientId { get; set; } = null!;

    [Column("client_secret")]
    public string? ClientSecret { get; set; }

    [Column("enable_refresh_token_rotation")]
    public bool EnableRefreshTokenRotation { get; set; }

    [Column("is_client_credentials_only")]
    public bool IsClientCredentialsOnly { get; set; }

    [ForeignKey("AppId")]
    [InverseProperty("StOauthClients")]
    public virtual StApp App { get; set; } = null!;

    [InverseProperty("StOauthClient")]
    public virtual ICollection<StOauthLogoutChallenge> StOauthLogoutChallenges { get; set; } = new List<StOauthLogoutChallenge>();

    [InverseProperty("StOauthClient")]
    public virtual ICollection<StOauthM2mToken> StOauthM2mTokens { get; set; } = new List<StOauthM2mToken>();

    [InverseProperty("StOauthClient")]
    public virtual ICollection<StOauthSession> StOauthSessions { get; set; } = new List<StOauthSession>();
}
