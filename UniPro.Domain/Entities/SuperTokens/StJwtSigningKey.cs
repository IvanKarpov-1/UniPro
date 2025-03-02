using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "KeyId")]
[Table("st_jwt_signing_keys")]
[Index("AppId", Name = "jwt_signing_keys_app_id_index")]
public partial class StJwtSigningKey
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("key_id")]
    [StringLength(255)]
    public string KeyId { get; set; } = null!;

    [Column("key_string")]
    public string KeyString { get; set; } = null!;

    [Column("algorithm")]
    [StringLength(10)]
    public string Algorithm { get; set; } = null!;

    [Column("created_at")]
    public long? CreatedAt { get; set; }

    [ForeignKey("AppId")]
    [InverseProperty("StJwtSigningKeys")]
    public virtual StApp App { get; set; } = null!;
}
