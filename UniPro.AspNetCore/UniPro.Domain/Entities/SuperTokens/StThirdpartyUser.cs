using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "UserId")]
[Table("st_thirdparty_users")]
[Index("AppId", "Email", Name = "thirdparty_users_email_index")]
[Index("AppId", "ThirdPartyId", "ThirdPartyUserId", Name = "thirdparty_users_thirdparty_user_id_index")]
public partial class StThirdpartyUser
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Column("third_party_id")]
    [StringLength(28)]
    public string ThirdPartyId { get; set; } = null!;

    [Column("third_party_user_id")]
    [StringLength(256)]
    public string ThirdPartyUserId { get; set; } = null!;

    [Key]
    [Column("user_id")]
    [StringLength(36)]
    public string UserId { get; set; } = null!;

    [Column("email")]
    [StringLength(256)]
    public string Email { get; set; } = null!;

    [Column("time_joined")]
    public long TimeJoined { get; set; }

    [ForeignKey("AppId, UserId")]
    [InverseProperty("StThirdpartyUser")]
    public virtual StAppIdToUserId StAppIdToUserId { get; set; } = null!;
}
