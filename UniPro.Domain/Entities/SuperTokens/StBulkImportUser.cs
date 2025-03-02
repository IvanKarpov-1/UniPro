using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "Id")]
[Table("st_bulk_import_users")]
[Index("AppId", "Status", "CreatedAt", "Id", Name = "bulk_import_users_pagination_index1", IsDescending = new[] { false, false, true, true })]
[Index("AppId", "CreatedAt", "Id", Name = "bulk_import_users_pagination_index2", IsDescending = new[] { false, true, true })]
[Index("AppId", "Status", "UpdatedAt", Name = "bulk_import_users_status_updated_at_index")]
public partial class StBulkImportUser
{
    [Key]
    [Column("id")]
    [StringLength(36)]
    public string Id { get; set; } = null!;

    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Column("primary_user_id")]
    [StringLength(36)]
    public string? PrimaryUserId { get; set; }

    [Column("raw_data")]
    public string RawData { get; set; } = null!;

    [Column("status")]
    [StringLength(128)]
    public string? Status { get; set; }

    [Column("error_msg")]
    public string? ErrorMsg { get; set; }

    [Column("created_at")]
    public long CreatedAt { get; set; }

    [Column("updated_at")]
    public long UpdatedAt { get; set; }

    [ForeignKey("AppId")]
    [InverseProperty("StBulkImportUsers")]
    public virtual StApp App { get; set; } = null!;
}
