using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "TenantId", "UserId")]
[Table("st_emailpassword_user_to_tenant")]
[Index("AppId", "TenantId", "Email", Name = "emailpassword_user_to_tenant_email_index")]
[Index("AppId", "TenantId", "Email", Name = "st_emailpassword_user_to_tenant_email_key", IsUnique = true)]
public partial class StEmailpasswordUserToTenant
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
    [StringLength(36)]
    public string UserId { get; set; } = null!;

    [Column("email")]
    [StringLength(256)]
    public string Email { get; set; } = null!;

    [ForeignKey("AppId, TenantId, UserId")]
    [InverseProperty("StEmailpasswordUserToTenant")]
    public virtual StAllAuthRecipeUser StAllAuthRecipeUser { get; set; } = null!;
}
