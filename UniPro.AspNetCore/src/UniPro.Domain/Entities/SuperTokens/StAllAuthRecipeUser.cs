using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace UniPro.Domain.Entities.SuperTokens;

[PrimaryKey("AppId", "TenantId", "UserId")]
[Table("st_all_auth_recipe_users")]
[Index("AppId", "TenantId", Name = "all_auth_recipe_tenant_id_index")]
[Index("AppId", Name = "all_auth_recipe_user_app_id_index")]
[Index("AppId", "UserId", Name = "all_auth_recipe_user_id_app_id_index")]
[Index("UserId", Name = "all_auth_recipe_user_id_index")]
[Index("AppId", "TenantId", "PrimaryOrRecipeUserTimeJoined", "PrimaryOrRecipeUserId", Name = "all_auth_recipe_users_pagination_index1", IsDescending = new[] { false, false, true, true })]
[Index("AppId", "TenantId", "PrimaryOrRecipeUserTimeJoined", "PrimaryOrRecipeUserId", Name = "all_auth_recipe_users_pagination_index2", IsDescending = new[] { false, false, false, true })]
[Index("RecipeId", "AppId", "TenantId", "PrimaryOrRecipeUserTimeJoined", "PrimaryOrRecipeUserId", Name = "all_auth_recipe_users_pagination_index3", IsDescending = new[] { false, false, false, true, true })]
[Index("RecipeId", "AppId", "TenantId", "PrimaryOrRecipeUserTimeJoined", "PrimaryOrRecipeUserId", Name = "all_auth_recipe_users_pagination_index4", IsDescending = new[] { false, false, false, false, true })]
[Index("PrimaryOrRecipeUserId", "AppId", Name = "all_auth_recipe_users_primary_user_id_index")]
[Index("AppId", "RecipeId", "TenantId", Name = "all_auth_recipe_users_recipe_id_index")]
public partial class StAllAuthRecipeUser
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
    [Column("user_id")]
    [StringLength(36)]
    public string UserId { get; set; } = null!;

    [Column("primary_or_recipe_user_id")]
    [StringLength(36)]
    public string PrimaryOrRecipeUserId { get; set; } = null!;

    [Column("is_linked_or_is_a_primary_user")]
    public bool IsLinkedOrIsAPrimaryUser { get; set; }

    [Column("recipe_id")]
    [StringLength(128)]
    public string RecipeId { get; set; } = null!;

    [Column("time_joined")]
    public long TimeJoined { get; set; }

    [Column("primary_or_recipe_user_time_joined")]
    public long PrimaryOrRecipeUserTimeJoined { get; set; }

    [ForeignKey("AppId, PrimaryOrRecipeUserId")]
    [InverseProperty("StAllAuthRecipeUserStAppIdToUserIds")]
    public virtual StAppIdToUserId StAppIdToUserId { get; set; } = null!;

    [ForeignKey("AppId, UserId")]
    [InverseProperty("StAllAuthRecipeUserStAppIdToUserIdNavigations")]
    public virtual StAppIdToUserId StAppIdToUserIdNavigation { get; set; } = null!;

    [InverseProperty("StAllAuthRecipeUser")]
    public virtual StEmailpasswordUserToTenant? StEmailpasswordUserToTenant { get; set; }

    [InverseProperty("StAllAuthRecipeUser")]
    public virtual StPasswordlessUserToTenant? StPasswordlessUserToTenant { get; set; }

    [ForeignKey("AppId, TenantId")]
    [InverseProperty("StAllAuthRecipeUsers")]
    public virtual StTenant StTenant { get; set; } = null!;

    [InverseProperty("StAllAuthRecipeUser")]
    public virtual StThirdpartyUserToTenant? StThirdpartyUserToTenant { get; set; }
}
