using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "TenantId", "UserId")]
[Table("st_passwordless_user_to_tenant")]
[Index("AppId", "TenantId", "Email", Name = "passwordless_user_to_tenant_email_index")]
[Index("AppId", "TenantId", "PhoneNumber", Name = "passwordless_user_to_tenant_phone_number_index")]
[Index("AppId", "TenantId", "Email", Name = "st_passwordless_user_to_tenant_email_key", IsUnique = true)]
[Index("AppId", "TenantId", "PhoneNumber", Name = "st_passwordless_user_to_tenant_phone_number_key", IsUnique = true)]
public partial class StPasswordlessUserToTenant
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
    public string? Email { get; set; }

    [Column("phone_number")]
    [StringLength(256)]
    public string? PhoneNumber { get; set; }

    [ForeignKey("AppId, TenantId, UserId")]
    [InverseProperty("StPasswordlessUserToTenant")]
    public virtual StAllAuthRecipeUser StAllAuthRecipeUser { get; set; } = null!;
}
