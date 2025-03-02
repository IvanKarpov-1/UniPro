using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "Challenge")]
[Table("st_oauth_logout_challenges")]
[Index("TimeCreated", Name = "oauth_logout_challenges_time_created_index", AllDescending = true)]
public partial class StOauthLogoutChallenge
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("challenge")]
    [StringLength(128)]
    public string Challenge { get; set; } = null!;

    [Column("client_id")]
    [StringLength(255)]
    public string ClientId { get; set; } = null!;

    [Column("post_logout_redirect_uri")]
    [StringLength(1024)]
    public string? PostLogoutRedirectUri { get; set; }

    [Column("session_handle")]
    [StringLength(128)]
    public string? SessionHandle { get; set; }

    [Column("state")]
    [StringLength(128)]
    public string? State { get; set; }

    [Column("time_created")]
    public long TimeCreated { get; set; }

    [ForeignKey("AppId, ClientId")]
    [InverseProperty("StOauthLogoutChallenges")]
    public virtual StOauthClient StOauthClient { get; set; } = null!;
}
