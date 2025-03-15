using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "UserId")]
[Table("st_user_metadata")]
[Index("AppId", Name = "user_metadata_app_id_index")]
public partial class StUserMetadatum
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("user_id")]
    [StringLength(128)]
    public string UserId { get; set; } = null!;

    [Column("user_metadata")]
    public string UserMetadata { get; set; } = null!;

    [ForeignKey("AppId")]
    [InverseProperty("StUserMetadata")]
    public virtual StApp App { get; set; } = null!;
}
