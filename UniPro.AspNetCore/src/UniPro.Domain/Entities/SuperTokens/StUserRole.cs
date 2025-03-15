using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "TenantId", "UserId", "Role")]
[Table("st_user_roles")]
[Index("AppId", "Role", Name = "user_roles_app_id_role_index")]
[Index("AppId", "TenantId", "Role", Name = "user_roles_role_index")]
[Index("AppId", "TenantId", Name = "user_roles_tenant_id_index")]
public partial class StUserRole
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
    [Column("role")]
    [StringLength(255)]
    public string Role { get; set; } = null!;

    [ForeignKey("AppId, TenantId")]
    [InverseProperty("StUserRoles")]
    public virtual StTenant StTenant { get; set; } = null!;
}
