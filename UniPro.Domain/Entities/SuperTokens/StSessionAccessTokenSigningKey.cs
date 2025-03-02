using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "CreatedAtTime")]
[Table("st_session_access_token_signing_keys")]
[Index("AppId", Name = "access_token_signing_keys_app_id_index")]
public partial class StSessionAccessTokenSigningKey
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("created_at_time")]
    public long CreatedAtTime { get; set; }

    [Column("value")]
    public string? Value { get; set; }

    [ForeignKey("AppId")]
    [InverseProperty("StSessionAccessTokenSigningKeys")]
    public virtual StApp App { get; set; } = null!;
}
