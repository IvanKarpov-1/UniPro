using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "Role")]
[Table("st_roles")]
[Index("AppId", Name = "roles_app_id_index")]
public partial class StRole
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("role")]
    [StringLength(255)]
    public string Role { get; set; } = null!;

    [ForeignKey("AppId")]
    [InverseProperty("StRoles")]
    public virtual StApp App { get; set; } = null!;

    [InverseProperty("StRole")]
    public virtual ICollection<StRolePermission> StRolePermissions { get; set; } = new List<StRolePermission>();
}
