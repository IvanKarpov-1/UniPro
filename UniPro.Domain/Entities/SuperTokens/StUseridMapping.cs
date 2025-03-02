using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "SupertokensUserId", "ExternalUserId")]
[Table("st_userid_mapping")]
[Index("AppId", "ExternalUserId", Name = "st_userid_mapping_external_user_id_key", IsUnique = true)]
[Index("AppId", "SupertokensUserId", Name = "st_userid_mapping_supertokens_user_id_key", IsUnique = true)]
[Index("AppId", "SupertokensUserId", Name = "userid_mapping_supertokens_user_id_index")]
public partial class StUseridMapping
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("supertokens_user_id")]
    [StringLength(36)]
    public string SupertokensUserId { get; set; } = null!;

    [Key]
    [Column("external_user_id")]
    [StringLength(128)]
    public string ExternalUserId { get; set; } = null!;

    [Column("external_user_id_info")]
    public string? ExternalUserIdInfo { get; set; }

    [ForeignKey("AppId, SupertokensUserId")]
    [InverseProperty("StUseridMapping")]
    public virtual StAppIdToUserId StAppIdToUserId { get; set; } = null!;
}
