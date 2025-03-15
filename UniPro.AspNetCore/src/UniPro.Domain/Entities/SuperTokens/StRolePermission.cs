using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "Role", "Permission")]
[Table("st_role_permissions")]
[Index("AppId", "Permission", Name = "role_permissions_permission_index")]
[Index("AppId", "Role", Name = "role_permissions_role_index")]
public partial class StRolePermission
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("role")]
    [StringLength(255)]
    public string Role { get; set; } = null!;

    [Key]
    [Column("permission")]
    [StringLength(255)]
    public string Permission { get; set; } = null!;

    [ForeignKey("AppId, Role")]
    [InverseProperty("StRolePermissions")]
    public virtual StRole StRole { get; set; } = null!;
}
