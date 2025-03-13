using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "TenantId", "Name")]
[Table("st_key_value")]
[Index("AppId", "TenantId", Name = "key_value_tenant_id_index")]
public partial class StKeyValue
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
    [Column("name")]
    [StringLength(128)]
    public string Name { get; set; } = null!;

    [Column("value")]
    public string? Value { get; set; }

    [Column("created_at_time")]
    public long? CreatedAtTime { get; set; }

    [ForeignKey("AppId, TenantId")]
    [InverseProperty("StKeyValues")]
    public virtual StTenant StTenant { get; set; } = null!;
}
