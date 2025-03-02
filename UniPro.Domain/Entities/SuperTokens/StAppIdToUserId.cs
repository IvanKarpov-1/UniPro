using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "UserId")]
[Table("st_app_id_to_user_id")]
[Index("AppId", Name = "app_id_to_user_id_app_id_index")]
[Index("PrimaryOrRecipeUserId", "AppId", Name = "app_id_to_user_id_primary_user_id_index")]
[Index("UserId", "AppId", Name = "app_id_to_user_id_user_id_index")]
public partial class StAppIdToUserId
{
    [Key]
    [Column("app_id")]
    [StringLength(64)]
    public string AppId { get; set; } = null!;

    [Key]
    [Column("user_id")]
    [StringLength(36)]
    public string UserId { get; set; } = null!;

    [Column("recipe_id")]
    [StringLength(128)]
    public string RecipeId { get; set; } = null!;

    [Column("primary_or_recipe_user_id")]
    [StringLength(36)]
    public string PrimaryOrRecipeUserId { get; set; } = null!;

    [Column("is_linked_or_is_a_primary_user")]
    public bool IsLinkedOrIsAPrimaryUser { get; set; }

    [ForeignKey("AppId")]
    [InverseProperty("StAppIdToUserIds")]
    public virtual StApp App { get; set; } = null!;

    [InverseProperty("StAppIdToUserIdNavigation")]
    public virtual ICollection<StAppIdToUserId> InverseStAppIdToUserIdNavigation { get; set; } = new List<StAppIdToUserId>();

    [InverseProperty("StAppIdToUserIdNavigation")]
    public virtual ICollection<StAllAuthRecipeUser> StAllAuthRecipeUserStAppIdToUserIdNavigations { get; set; } = new List<StAllAuthRecipeUser>();

    [InverseProperty("StAppIdToUserId")]
    public virtual ICollection<StAllAuthRecipeUser> StAllAuthRecipeUserStAppIdToUserIds { get; set; } = new List<StAllAuthRecipeUser>();

    [ForeignKey("AppId, PrimaryOrRecipeUserId")]
    [InverseProperty("InverseStAppIdToUserIdNavigation")]
    public virtual StAppIdToUserId StAppIdToUserIdNavigation { get; set; } = null!;

    [InverseProperty("StAppIdToUserId")]
    public virtual ICollection<StEmailpasswordPswdResetToken> StEmailpasswordPswdResetTokens { get; set; } = new List<StEmailpasswordPswdResetToken>();

    [InverseProperty("StAppIdToUserId")]
    public virtual StEmailpasswordUser? StEmailpasswordUser { get; set; }

    [InverseProperty("StAppIdToUserId")]
    public virtual StPasswordlessUser? StPasswordlessUser { get; set; }

    [InverseProperty("StAppIdToUserId")]
    public virtual StThirdpartyUser? StThirdpartyUser { get; set; }

    [InverseProperty("StAppIdToUserId")]
    public virtual StUseridMapping? StUseridMapping { get; set; }
}
