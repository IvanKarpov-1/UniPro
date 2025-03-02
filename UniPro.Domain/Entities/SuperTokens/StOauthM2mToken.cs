using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "ClientId", "Iat")]
[Table("st_oauth_m2m_tokens")]
[Index("Exp", Name = "oauth_m2m_token_exp_index", AllDescending = true)]
[Index("Iat", "AppId", Name = "oauth_m2m_token_iat_index", AllDescending = true)]
public partial class StOauthM2mToken
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("client_id")]
    [StringLength(255)]
    public string ClientId { get; set; } = null!;

    [Key]
    [Column("iat")]
    public long Iat { get; set; }

    [Column("exp")]
    public long Exp { get; set; }

    [ForeignKey("AppId, ClientId")]
    [InverseProperty("StOauthM2mTokens")]
    public virtual StOauthClient StOauthClient { get; set; } = null!;
}
