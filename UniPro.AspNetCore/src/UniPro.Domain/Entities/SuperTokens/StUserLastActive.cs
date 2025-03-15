using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "UserId")]
[Table("st_user_last_active")]
[Index("AppId", Name = "user_last_active_app_id_index")]
[Index("LastActiveTime", "AppId", Name = "user_last_active_last_active_time_index", AllDescending = true)]
public partial class StUserLastActive
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("user_id")]
    [StringLength(128)]
    public string UserId { get; set; } = null!;

    [Column("last_active_time")]
    public long? LastActiveTime { get; set; }

    [ForeignKey("AppId")]
    [InverseProperty("StUserLastActives")]
    public virtual StApp App { get; set; } = null!;
}
