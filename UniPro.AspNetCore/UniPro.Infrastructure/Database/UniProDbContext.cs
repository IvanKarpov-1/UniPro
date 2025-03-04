using Microsoft.EntityFrameworkCore;
using UniPro.Domain.Entities;
using UniPro.Domain.Entities.SuperTokens;
using Task = UniPro.Domain.Entities.Task;

namespace UniPro.Infrastructure.Database;

public partial class UniProDbContext(DbContextOptions<UniProDbContext> options) : DbContext(options)
{
    public virtual DbSet<StAllAuthRecipeUser> StAllAuthRecipeUsers { get; set; }

    public virtual DbSet<StApp> StApps { get; set; }

    public virtual DbSet<StAppIdToUserId> StAppIdToUserIds { get; set; }

    public virtual DbSet<StBulkImportUser> StBulkImportUsers { get; set; }

    public virtual DbSet<StDashboardUser> StDashboardUsers { get; set; }

    public virtual DbSet<StDashboardUserSession> StDashboardUserSessions { get; set; }

    public virtual DbSet<StEmailpasswordPswdResetToken> StEmailpasswordPswdResetTokens { get; set; }

    public virtual DbSet<StEmailpasswordUser> StEmailpasswordUsers { get; set; }

    public virtual DbSet<StEmailpasswordUserToTenant> StEmailpasswordUserToTenants { get; set; }

    public virtual DbSet<StEmailverificationToken> StEmailverificationTokens { get; set; }

    public virtual DbSet<StEmailverificationVerifiedEmail> StEmailverificationVerifiedEmails { get; set; }

    public virtual DbSet<StJwtSigningKey> StJwtSigningKeys { get; set; }

    public virtual DbSet<StKeyValue> StKeyValues { get; set; }

    public virtual DbSet<StOauthClient> StOauthClients { get; set; }

    public virtual DbSet<StOauthLogoutChallenge> StOauthLogoutChallenges { get; set; }

    public virtual DbSet<StOauthM2mToken> StOauthM2mTokens { get; set; }

    public virtual DbSet<StOauthSession> StOauthSessions { get; set; }

    public virtual DbSet<StPasswordlessCode> StPasswordlessCodes { get; set; }

    public virtual DbSet<StPasswordlessDevice> StPasswordlessDevices { get; set; }

    public virtual DbSet<StPasswordlessUser> StPasswordlessUsers { get; set; }

    public virtual DbSet<StPasswordlessUserToTenant> StPasswordlessUserToTenants { get; set; }

    public virtual DbSet<StRole> StRoles { get; set; }

    public virtual DbSet<StRolePermission> StRolePermissions { get; set; }

    public virtual DbSet<StSessionAccessTokenSigningKey> StSessionAccessTokenSigningKeys { get; set; }

    public virtual DbSet<StSessionInfo> StSessionInfos { get; set; }

    public virtual DbSet<StTenant> StTenants { get; set; }

    public virtual DbSet<StTenantConfig> StTenantConfigs { get; set; }

    public virtual DbSet<StTenantFirstFactor> StTenantFirstFactors { get; set; }

    public virtual DbSet<StTenantRequiredSecondaryFactor> StTenantRequiredSecondaryFactors { get; set; }

    public virtual DbSet<StTenantThirdpartyProvider> StTenantThirdpartyProviders { get; set; }

    public virtual DbSet<StTenantThirdpartyProviderClient> StTenantThirdpartyProviderClients { get; set; }

    public virtual DbSet<StThirdpartyUser> StThirdpartyUsers { get; set; }

    public virtual DbSet<StThirdpartyUserToTenant> StThirdpartyUserToTenants { get; set; }

    public virtual DbSet<StTotpUsedCode> StTotpUsedCodes { get; set; }

    public virtual DbSet<StTotpUser> StTotpUsers { get; set; }

    public virtual DbSet<StTotpUserDevice> StTotpUserDevices { get; set; }

    public virtual DbSet<StUserLastActive> StUserLastActives { get; set; }

    public virtual DbSet<StUserMetadatum> StUserMetadata { get; set; }

    public virtual DbSet<StUserRole> StUserRoles { get; set; }

    public virtual DbSet<StUseridMapping> StUseridMappings { get; set; }
    
    //---------------------------------------------
    public virtual DbSet<User> Users { get; set; }
    
    public virtual DbSet<StudentInfo> StudentInfos { get; set; }
    
    public virtual DbSet<StudentGroup> StudentGroups { get; set; }
    
    public virtual DbSet<Department> Departments { get; set; }
    
    public virtual DbSet<Academic> Academics { get; set; }
    
    public virtual DbSet<University> Universities { get; set; }
    
    public virtual DbSet<TeacherInfo> TeacherInfos { get; set; }
    
    public virtual DbSet<Course> Courses { get; set; }
    
    public virtual DbSet<Task> Tasks { get; set; }
    
    public virtual DbSet<TaskType> TaskTypes { get; set; }
    
    public virtual DbSet<StudentTask> StudentTasks { get; set; }
    
    public virtual DbSet<Grade> Grades { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StAllAuthRecipeUser>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.TenantId, e.UserId }).HasName("st_all_auth_recipe_users_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.TenantId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.UserId).IsFixedLength();
            entity.Property(e => e.IsLinkedOrIsAPrimaryUser).HasDefaultValue(false);
            entity.Property(e => e.PrimaryOrRecipeUserId).IsFixedLength();

            entity.HasOne(d => d.StAppIdToUserId).WithMany(p => p.StAllAuthRecipeUserStAppIdToUserIds).HasConstraintName("st_all_auth_recipe_users_primary_or_recipe_user_id_fkey");

            entity.HasOne(d => d.StTenant).WithMany(p => p.StAllAuthRecipeUsers).HasConstraintName("st_all_auth_recipe_users_tenant_id_fkey");

            entity.HasOne(d => d.StAppIdToUserIdNavigation).WithMany(p => p.StAllAuthRecipeUserStAppIdToUserIdNavigations).HasConstraintName("st_all_auth_recipe_users_user_id_fkey");
        });

        modelBuilder.Entity<StApp>(entity =>
        {
            entity.HasKey(e => e.AppId).HasName("st_apps_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
        });

        modelBuilder.Entity<StAppIdToUserId>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.UserId }).HasName("st_app_id_to_user_id_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.UserId).IsFixedLength();
            entity.Property(e => e.IsLinkedOrIsAPrimaryUser).HasDefaultValue(false);
            entity.Property(e => e.PrimaryOrRecipeUserId).IsFixedLength();

            entity.HasOne(d => d.App).WithMany(p => p.StAppIdToUserIds).HasConstraintName("st_app_id_to_user_id_app_id_fkey");

            entity.HasOne(d => d.StAppIdToUserIdNavigation).WithMany(p => p.InverseStAppIdToUserIdNavigation).HasConstraintName("st_app_id_to_user_id_primary_or_recipe_user_id_fkey");
        });

        modelBuilder.Entity<StBulkImportUser>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.Id }).HasName("st_bulk_import_users_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.Id).IsFixedLength();
            entity.Property(e => e.Status).HasDefaultValueSql("'NEW'::character varying");

            entity.HasOne(d => d.App).WithMany(p => p.StBulkImportUsers).HasConstraintName("st_bulk_import_users_app_id_fkey");
        });

        modelBuilder.Entity<StDashboardUser>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.UserId }).HasName("st_dashboard_users_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.UserId).IsFixedLength();

            entity.HasOne(d => d.App).WithMany(p => p.StDashboardUsers).HasConstraintName("st_dashboard_users_app_id_fkey");
        });

        modelBuilder.Entity<StDashboardUserSession>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.SessionId }).HasName("st_dashboard_user_sessions_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.SessionId).IsFixedLength();
            entity.Property(e => e.UserId).IsFixedLength();

            entity.HasOne(d => d.StDashboardUser).WithMany(p => p.StDashboardUserSessions).HasConstraintName("st_dashboard_user_sessions_user_id_fkey");
        });

        modelBuilder.Entity<StEmailpasswordPswdResetToken>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.UserId, e.Token }).HasName("st_emailpassword_pswd_reset_tokens_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.UserId).IsFixedLength();

            entity.HasOne(d => d.StAppIdToUserId).WithMany(p => p.StEmailpasswordPswdResetTokens).HasConstraintName("st_emailpassword_pswd_reset_tokens_user_id_fkey");
        });

        modelBuilder.Entity<StEmailpasswordUser>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.UserId }).HasName("st_emailpassword_users_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.UserId).IsFixedLength();

            entity.HasOne(d => d.StAppIdToUserId).WithOne(p => p.StEmailpasswordUser).HasConstraintName("st_emailpassword_users_user_id_fkey");
        });

        modelBuilder.Entity<StEmailpasswordUserToTenant>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.TenantId, e.UserId }).HasName("st_emailpassword_user_to_tenant_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.TenantId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.UserId).IsFixedLength();

            entity.HasOne(d => d.StAllAuthRecipeUser).WithOne(p => p.StEmailpasswordUserToTenant).HasConstraintName("st_emailpassword_user_to_tenant_user_id_fkey");
        });

        modelBuilder.Entity<StEmailverificationToken>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.TenantId, e.UserId, e.Email, e.Token }).HasName("st_emailverification_tokens_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.TenantId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.StTenant).WithMany(p => p.StEmailverificationTokens).HasConstraintName("st_emailverification_tokens_tenant_id_fkey");
        });

        modelBuilder.Entity<StEmailverificationVerifiedEmail>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.UserId, e.Email }).HasName("st_emailverification_verified_emails_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.App).WithMany(p => p.StEmailverificationVerifiedEmails).HasConstraintName("st_emailverification_verified_emails_app_id_fkey");
        });

        modelBuilder.Entity<StJwtSigningKey>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.KeyId }).HasName("st_jwt_signing_keys_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.App).WithMany(p => p.StJwtSigningKeys).HasConstraintName("st_jwt_signing_keys_app_id_fkey");
        });

        modelBuilder.Entity<StKeyValue>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.TenantId, e.Name }).HasName("st_key_value_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.TenantId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.StTenant).WithMany(p => p.StKeyValues).HasConstraintName("st_key_value_tenant_id_fkey");
        });

        modelBuilder.Entity<StOauthClient>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.ClientId }).HasName("st_oauth_clients_pkey");

            entity.HasOne(d => d.App).WithMany(p => p.StOauthClients).HasConstraintName("st_oauth_clients_app_id_fkey");
        });

        modelBuilder.Entity<StOauthLogoutChallenge>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.Challenge }).HasName("st_oauth_logout_challenges_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.StOauthClient).WithMany(p => p.StOauthLogoutChallenges).HasConstraintName("st_oauth_logout_challenges_client_id_fkey");
        });

        modelBuilder.Entity<StOauthM2mToken>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.ClientId, e.Iat }).HasName("st_oauth_m2m_tokens_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.StOauthClient).WithMany(p => p.StOauthM2mTokens).HasConstraintName("st_oauth_m2m_tokens_client_id_fkey");
        });

        modelBuilder.Entity<StOauthSession>(entity =>
        {
            entity.HasKey(e => e.Gid).HasName("st_oauth_sessions_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.StOauthClient).WithMany(p => p.StOauthSessions)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("st_oauth_sessions_client_id_fkey");
        });

        modelBuilder.Entity<StPasswordlessCode>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.TenantId, e.CodeId }).HasName("st_passwordless_codes_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.TenantId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.CodeId).IsFixedLength();
            entity.Property(e => e.DeviceIdHash).IsFixedLength();
            entity.Property(e => e.LinkCodeHash).IsFixedLength();

            entity.HasOne(d => d.StPasswordlessDevice).WithMany(p => p.StPasswordlessCodes).HasConstraintName("st_passwordless_codes_device_id_hash_fkey");
        });

        modelBuilder.Entity<StPasswordlessDevice>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.TenantId, e.DeviceIdHash }).HasName("st_passwordless_devices_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.TenantId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.DeviceIdHash).IsFixedLength();
            entity.Property(e => e.LinkCodeSalt).IsFixedLength();

            entity.HasOne(d => d.StTenant).WithMany(p => p.StPasswordlessDevices).HasConstraintName("st_passwordless_devices_tenant_id_fkey");
        });

        modelBuilder.Entity<StPasswordlessUser>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.UserId }).HasName("st_passwordless_users_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.UserId).IsFixedLength();

            entity.HasOne(d => d.StAppIdToUserId).WithOne(p => p.StPasswordlessUser).HasConstraintName("st_passwordless_users_user_id_fkey");
        });

        modelBuilder.Entity<StPasswordlessUserToTenant>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.TenantId, e.UserId }).HasName("st_passwordless_user_to_tenant_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.TenantId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.UserId).IsFixedLength();

            entity.HasOne(d => d.StAllAuthRecipeUser).WithOne(p => p.StPasswordlessUserToTenant).HasConstraintName("st_passwordless_user_to_tenant_user_id_fkey");
        });

        modelBuilder.Entity<StRole>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.Role }).HasName("st_roles_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.App).WithMany(p => p.StRoles).HasConstraintName("st_roles_app_id_fkey");
        });

        modelBuilder.Entity<StRolePermission>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.Role, e.Permission }).HasName("st_role_permissions_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.StRole).WithMany(p => p.StRolePermissions).HasConstraintName("st_role_permissions_role_fkey");
        });

        modelBuilder.Entity<StSessionAccessTokenSigningKey>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.CreatedAtTime }).HasName("st_session_access_token_signing_keys_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.App).WithMany(p => p.StSessionAccessTokenSigningKeys).HasConstraintName("st_session_access_token_signing_keys_app_id_fkey");
        });

        modelBuilder.Entity<StSessionInfo>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.TenantId, e.SessionHandle }).HasName("st_session_info_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.TenantId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.StTenant).WithMany(p => p.StSessionInfos).HasConstraintName("st_session_info_tenant_id_fkey");
        });

        modelBuilder.Entity<StTenant>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.TenantId }).HasName("st_tenants_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.TenantId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.App).WithMany(p => p.StTenants).HasConstraintName("st_tenants_app_id_fkey");
        });

        modelBuilder.Entity<StTenantConfig>(entity =>
        {
            entity.HasKey(e => new { e.ConnectionUriDomain, e.AppId, e.TenantId }).HasName("st_tenant_configs_pkey");

            entity.Property(e => e.ConnectionUriDomain).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.TenantId).HasDefaultValueSql("'public'::character varying");
        });

        modelBuilder.Entity<StTenantFirstFactor>(entity =>
        {
            entity.HasKey(e => new { e.ConnectionUriDomain, e.AppId, e.TenantId, e.FactorId }).HasName("st_tenant_first_factors_pkey");

            entity.Property(e => e.ConnectionUriDomain).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.TenantId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.StTenantConfig).WithMany(p => p.StTenantFirstFactors).HasConstraintName("st_tenant_first_factors_tenant_id_fkey");
        });

        modelBuilder.Entity<StTenantRequiredSecondaryFactor>(entity =>
        {
            entity.HasKey(e => new { e.ConnectionUriDomain, e.AppId, e.TenantId, e.FactorId }).HasName("st_tenant_required_secondary_factors_pkey");

            entity.Property(e => e.ConnectionUriDomain).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.TenantId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.StTenantConfig).WithMany(p => p.StTenantRequiredSecondaryFactors).HasConstraintName("st_tenant_required_secondary_factors_tenant_id_fkey");
        });

        modelBuilder.Entity<StTenantThirdpartyProvider>(entity =>
        {
            entity.HasKey(e => new { e.ConnectionUriDomain, e.AppId, e.TenantId, e.ThirdPartyId }).HasName("st_tenant_thirdparty_providers_pkey");

            entity.Property(e => e.ConnectionUriDomain).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.TenantId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.StTenantConfig).WithMany(p => p.StTenantThirdpartyProviders).HasConstraintName("st_tenant_thirdparty_providers_tenant_id_fkey");
        });

        modelBuilder.Entity<StTenantThirdpartyProviderClient>(entity =>
        {
            entity.HasKey(e => new { e.ConnectionUriDomain, e.AppId, e.TenantId, e.ThirdPartyId, e.ClientType }).HasName("st_tenant_thirdparty_provider_clients_pkey");

            entity.Property(e => e.ConnectionUriDomain).HasDefaultValueSql("''::character varying");
            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.TenantId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.ClientType).HasDefaultValueSql("''::character varying");

            entity.HasOne(d => d.StTenantThirdpartyProvider).WithMany(p => p.StTenantThirdpartyProviderClients).HasConstraintName("st_tenant_thirdparty_provider_clients_third_party_id_fkey");
        });

        modelBuilder.Entity<StThirdpartyUser>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.UserId }).HasName("st_thirdparty_users_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.UserId).IsFixedLength();

            entity.HasOne(d => d.StAppIdToUserId).WithOne(p => p.StThirdpartyUser).HasConstraintName("st_thirdparty_users_user_id_fkey");
        });

        modelBuilder.Entity<StThirdpartyUserToTenant>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.TenantId, e.UserId }).HasName("st_thirdparty_user_to_tenant_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.TenantId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.UserId).IsFixedLength();

            entity.HasOne(d => d.StAllAuthRecipeUser).WithOne(p => p.StThirdpartyUserToTenant).HasConstraintName("st_thirdparty_user_to_tenant_user_id_fkey");
        });

        modelBuilder.Entity<StTotpUsedCode>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.TenantId, e.UserId, e.CreatedTimeMs }).HasName("st_totp_used_codes_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.TenantId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.StTenant).WithMany(p => p.StTotpUsedCodes).HasConstraintName("st_totp_used_codes_tenant_id_fkey");

            entity.HasOne(d => d.StTotpUser).WithMany(p => p.StTotpUsedCodes).HasConstraintName("st_totp_used_codes_user_id_fkey");
        });

        modelBuilder.Entity<StTotpUser>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.UserId }).HasName("st_totp_users_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.App).WithMany(p => p.StTotpUsers).HasConstraintName("st_totp_users_app_id_fkey");
        });

        modelBuilder.Entity<StTotpUserDevice>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.UserId, e.DeviceName }).HasName("st_totp_user_devices_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.StTotpUser).WithMany(p => p.StTotpUserDevices).HasConstraintName("st_totp_user_devices_user_id_fkey");
        });

        modelBuilder.Entity<StUserLastActive>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.UserId }).HasName("st_user_last_active_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.App).WithMany(p => p.StUserLastActives).HasConstraintName("st_user_last_active_app_id_fkey");
        });

        modelBuilder.Entity<StUserMetadatum>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.UserId }).HasName("st_user_metadata_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.App).WithMany(p => p.StUserMetadata).HasConstraintName("st_user_metadata_app_id_fkey");
        });

        modelBuilder.Entity<StUserRole>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.TenantId, e.UserId, e.Role }).HasName("st_user_roles_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.TenantId).HasDefaultValueSql("'public'::character varying");

            entity.HasOne(d => d.StTenant).WithMany(p => p.StUserRoles).HasConstraintName("st_user_roles_tenant_id_fkey");
        });

        modelBuilder.Entity<StUseridMapping>(entity =>
        {
            entity.HasKey(e => new { e.AppId, e.SupertokensUserId, e.ExternalUserId }).HasName("st_userid_mapping_pkey");

            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.SupertokensUserId).IsFixedLength();

            entity.HasOne(d => d.StAppIdToUserId).WithOne(p => p.StUseridMapping).HasConstraintName("st_userid_mapping_supertokens_user_id_fkey");
        });

        //---------------------------------------------
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("users_pkey");
        
            entity.Property(e => e.AppId).HasDefaultValueSql("'public'::character varying");
            entity.Property(e => e.UserId).IsFixedLength();
        
            entity.HasOne(d => d.StAppIdToUserId).WithMany(p => p.Users).HasConstraintName("st_users_user_id_fkey");
        });
        
        modelBuilder.Entity<StudentInfo>(entity =>
        {
            entity.HasKey(e => e.StudentId);
        
            entity.Property(e => e.StudentId).IsFixedLength();
        
            entity.HasOne(d => d.Student).WithOne(p => p.StudentInfo);
            entity.HasOne(d => d.StudentGroup).WithMany(p => p.StudentInfos);
            entity.HasOne(d => d.Department).WithMany(p => p.StudentInfos);
            entity.HasOne(d => d.Academic).WithMany(p => p.StudentInfos);
            entity.HasOne(d => d.University).WithMany(p => p.StudentInfos);
            entity.HasMany(d => d.StudentTasks).WithOne(p => p.Student);
        });
        
        modelBuilder.Entity<TeacherInfo>(entity =>
        {
            entity.HasKey(e => e.TeacherId);
        
            entity.Property(e => e.TeacherId).IsFixedLength();
        
            entity.HasOne(d => d.Teacher).WithOne(p => p.TeacherInfo);
            entity.HasOne(d => d.Department).WithMany(p => p.TeacherInfos);
            entity.HasOne(d => d.Academic).WithMany(p => p.TeacherInfos);
            entity.HasOne(d => d.University).WithMany(p => p.TeacherInfos);
        });

        modelBuilder.Entity<Task>(entity =>
        {
            entity.HasKey(e => new { e.TaskId });

            entity.Property(e => e.TeacherId).IsFixedLength();

            entity.HasOne(d => d.Course).WithMany(p => p.Tasks);
            entity.HasOne(d => d.TaskType).WithMany(p => p.Tasks);
            entity.HasOne(d => d.Teacher).WithMany(p => p.Tasks);
        });

        modelBuilder.Entity<StudentTask>(entity =>
        {
            entity.HasKey(e => new { e.StudentId, e.TaskId });

            entity.Property(e => e.StudentId).IsFixedLength();
            
            entity.HasOne(d => d.Grade).WithOne(p => p.StudentTask);
            entity.HasOne(d => d.Task).WithMany(p => p.StudentTasks);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}