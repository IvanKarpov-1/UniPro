using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "TenantId", "UserId", "Email", "Token")]
[Table("st_emailverification_tokens")]
[Index("TokenExpiry", Name = "emailverification_tokens_index")]
[Index("AppId", "TenantId", Name = "emailverification_tokens_tenant_id_index")]
[Index("Token", Name = "st_emailverification_tokens_token_key", IsUnique = true)]
public partial class StEmailverificationToken
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
    [StringLength(128)]
    public string UserId { get; set; } = null!;

    [Key]
    [Column("email")]
    [StringLength(256)]
    public string Email { get; set; } = null!;

    [Key]
    [Column("token")]
    [StringLength(128)]
    public string Token { get; set; } = null!;

    [Column("token_expiry")]
    public long TokenExpiry { get; set; }

    [ForeignKey("AppId, TenantId")]
    [InverseProperty("StEmailverificationTokens")]
    public virtual StTenant StTenant { get; set; } = null!;
}
