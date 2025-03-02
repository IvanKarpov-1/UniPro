using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "UserId", "Token")]
[Table("st_emailpassword_pswd_reset_tokens")]
[Index("TokenExpiry", Name = "emailpassword_password_reset_token_expiry_index")]
[Index("AppId", "UserId", Name = "emailpassword_pswd_reset_tokens_user_id_index")]
[Index("Token", Name = "st_emailpassword_pswd_reset_tokens_token_key", IsUnique = true)]
public partial class StEmailpasswordPswdResetToken
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("user_id")]
    [StringLength(36)]
    public string UserId { get; set; } = null!;

    [Key]
    [Column("token")]
    [StringLength(128)]
    public string Token { get; set; } = null!;

    [Column("email")]
    [StringLength(256)]
    public string? Email { get; set; }

    [Column("token_expiry")]
    public long TokenExpiry { get; set; }

    [ForeignKey("AppId, UserId")]
    [InverseProperty("StEmailpasswordPswdResetTokens")]
    public virtual StAppIdToUserId StAppIdToUserId { get; set; } = null!;
}
