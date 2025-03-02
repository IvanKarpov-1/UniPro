using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[Table("st_oauth_sessions")]
[Index("Exp", Name = "oauth_session_exp_index", AllDescending = true)]
[Index("AppId", "ExternalRefreshToken", Name = "oauth_session_external_refresh_token_index", IsDescending = new[] { false, true })]
[Index("ExternalRefreshToken", Name = "st_oauth_sessions_external_refresh_token_key", IsUnique = true)]
[Index("InternalRefreshToken", Name = "st_oauth_sessions_internal_refresh_token_key", IsUnique = true)]
public partial class StOauthSession
{
    [Key]
    [Column("gid")]
    [StringLength(255)]
    public string Gid { get; set; } = null!;

    [Column("app_id")]
    [StringLength(64)]
    public string? AppId { get; set; }

    [Column("client_id")]
    [StringLength(255)]
    public string ClientId { get; set; } = null!;

    [Column("session_handle")]
    [StringLength(128)]
    public string? SessionHandle { get; set; }

    [Column("external_refresh_token")]
    [StringLength(255)]
    public string? ExternalRefreshToken { get; set; }

    [Column("internal_refresh_token")]
    [StringLength(255)]
    public string? InternalRefreshToken { get; set; }

    [Column("jti")]
    public string Jti { get; set; } = null!;

    [Column("exp")]
    public long Exp { get; set; }

    [ForeignKey("AppId, ClientId")]
    [InverseProperty("StOauthSessions")]
    public virtual StOauthClient? StOauthClient { get; set; }
}
