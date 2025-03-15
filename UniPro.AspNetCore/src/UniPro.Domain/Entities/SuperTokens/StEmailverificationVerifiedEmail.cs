using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "UserId", "Email")]
[Table("st_emailverification_verified_emails")]
[Index("AppId", Name = "emailverification_verified_emails_app_id_index")]
public partial class StEmailverificationVerifiedEmail
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("user_id")]
    [StringLength(128)]
    public string UserId { get; set; } = null!;

    [Key]
    [Column("email")]
    [StringLength(256)]
    public string Email { get; set; } = null!;

    [ForeignKey("AppId")]
    [InverseProperty("StEmailverificationVerifiedEmails")]
    public virtual StApp App { get; set; } = null!;
}
