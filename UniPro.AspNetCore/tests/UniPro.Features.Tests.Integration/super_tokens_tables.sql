CREATE TABLE st_apps (
                      app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                      created_at_time BIGINT,
                      CONSTRAINT st_apps_pkey PRIMARY KEY (app_id)
);

CREATE TABLE st_tenants (
                         app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                         tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                         created_at_time BIGINT,
                         CONSTRAINT st_tenants_pkey PRIMARY KEY (app_id, tenant_id),
                         CONSTRAINT st_tenants_app_id_fkey FOREIGN KEY (app_id) REFERENCES public.st_apps(app_id) ON DELETE CASCADE
);

CREATE INDEX st_tenants_app_id_index ON st_tenants (app_id);

CREATE TABLE st_tenant_configs (
                                connection_uri_domain VARCHAR(256) DEFAULT '' NOT NULL,
                                app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                core_config TEXT,
                                email_password_enabled BOOLEAN,
                                passwordless_enabled BOOLEAN,
                                third_party_enabled BOOLEAN,
                                is_first_factors_null BOOLEAN,
                                CONSTRAINT st_tenant_configs_pkey PRIMARY KEY (connection_uri_domain, app_id, tenant_id)
);

CREATE TABLE st_tenant_thirdparty_providers (
                                             connection_uri_domain VARCHAR(256) DEFAULT '' NOT NULL,
                                             app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                             tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                             third_party_id VARCHAR(28) NOT NULL,
                                             name VARCHAR(64),
                                             authorization_endpoint TEXT,
                                             authorization_endpoint_query_params TEXT,
                                             token_endpoint TEXT,
                                             token_endpoint_body_params TEXT,
                                             user_info_endpoint TEXT,
                                             user_info_endpoint_query_params TEXT,
                                             user_info_endpoint_headers TEXT,
                                             jwks_uri TEXT,
                                             oidc_discovery_endpoint TEXT,
                                             require_email BOOLEAN,
                                             user_info_map_from_id_token_payload_user_id VARCHAR(64),
                                             user_info_map_from_id_token_payload_email VARCHAR(64),
                                             user_info_map_from_id_token_payload_email_verified VARCHAR(64),
                                             user_info_map_from_user_info_endpoint_user_id VARCHAR(64),
                                             user_info_map_from_user_info_endpoint_email VARCHAR(64),
                                             user_info_map_from_user_info_endpoint_email_verified VARCHAR(64),
                                             CONSTRAINT st_tenant_thirdparty_providers_pkey PRIMARY KEY (connection_uri_domain, app_id, tenant_id, third_party_id),
                                             CONSTRAINT st_tenant_thirdparty_providers_tenant_id_fkey FOREIGN KEY (connection_uri_domain, app_id, tenant_id) REFERENCES public.st_tenant_configs(connection_uri_domain, app_id, tenant_id) ON DELETE CASCADE
);

CREATE INDEX st_tenant_thirdparty_providers_tenant_id_index ON st_tenant_thirdparty_providers (connection_uri_domain, app_id, tenant_id);

CREATE TABLE st_tenant_thirdparty_provider_clients (
                                                    connection_uri_domain VARCHAR(256) DEFAULT '' NOT NULL,
                                                    app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                                    tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                                    third_party_id VARCHAR(28) NOT NULL,
                                                    client_type VARCHAR(64) DEFAULT '' NOT NULL,
                                                    client_id VARCHAR(256) NOT NULL,
                                                    client_secret TEXT,
                                                    scope VARCHAR(128)[],
                                                    force_pkce BOOLEAN,
                                                    additional_config TEXT,
                                                    CONSTRAINT st_tenant_thirdparty_provider_clients_pkey PRIMARY KEY (connection_uri_domain, app_id, tenant_id, third_party_id, client_type),
                                                    CONSTRAINT st_tenant_thirdparty_provider_clients_third_party_id_fkey FOREIGN KEY (connection_uri_domain, app_id, tenant_id, third_party_id) REFERENCES public.st_tenant_thirdparty_providers(connection_uri_domain, app_id, tenant_id, third_party_id) ON DELETE CASCADE
);

CREATE INDEX st_tenant_thirdparty_provider_clients_third_party_id_index ON st_tenant_thirdparty_provider_clients (connection_uri_domain, app_id, tenant_id, third_party_id);

CREATE TABLE st_tenant_first_factors (
                                      connection_uri_domain VARCHAR(256) DEFAULT '' NOT NULL,
                                      app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                      tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                      factor_id VARCHAR(128),
                                      CONSTRAINT st_tenant_first_factors_pkey PRIMARY KEY (connection_uri_domain, app_id, tenant_id, factor_id),
                                      CONSTRAINT st_tenant_first_factors_tenant_id_fkey FOREIGN KEY (connection_uri_domain, app_id, tenant_id) REFERENCES public.st_tenant_configs(connection_uri_domain, app_id, tenant_id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS st_tenant_first_factors_tenant_id_index ON st_tenant_first_factors (connection_uri_domain, app_id, tenant_id);

CREATE TABLE st_tenant_required_secondary_factors (
                                                   connection_uri_domain VARCHAR(256) DEFAULT '' NOT NULL,
                                                   app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                                   tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                                   factor_id VARCHAR(128),
                                                   CONSTRAINT st_tenant_required_secondary_factors_pkey PRIMARY KEY (connection_uri_domain, app_id, tenant_id, factor_id),
                                                   CONSTRAINT st_tenant_required_secondary_factors_tenant_id_fkey FOREIGN KEY (connection_uri_domain, app_id, tenant_id) REFERENCES public.st_tenant_configs(connection_uri_domain, app_id, tenant_id) ON DELETE CASCADE
);

CREATE INDEX IF NOT EXISTS tenant_default_required_factor_ids_tenant_id_index ON st_tenant_required_secondary_factors (connection_uri_domain, app_id, tenant_id);

CREATE TABLE st_key_value (
                           app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                           tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                           name VARCHAR(128) NOT NULL,
                           value TEXT,
                           created_at_time BIGINT,
                           CONSTRAINT st_key_value_pkey PRIMARY KEY (app_id, tenant_id, name),
                           CONSTRAINT st_key_value_tenant_id_fkey FOREIGN KEY (app_id, tenant_id) REFERENCES public.st_tenants(app_id, tenant_id) ON DELETE CASCADE
);

CREATE INDEX st_key_value_tenant_id_index ON st_key_value (app_id, tenant_id);

CREATE TABLE st_app_id_to_user_id (
                                   app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                   user_id character(36) NOT NULL,
                                   recipe_id VARCHAR(128) NOT NULL,
                                   primary_or_recipe_user_id CHAR(36) NOT NULL,
                                   is_linked_or_is_a_primary_user BOOLEAN NOT NULL DEFAULT FALSE,
                                   CONSTRAINT st_app_id_to_user_id_pkey PRIMARY KEY (app_id, user_id),
                                   CONSTRAINT st_app_id_to_user_id_primary_or_recipe_user_id_fkey FOREIGN KEY(app_id, primary_or_recipe_user_id) REFERENCES st_app_id_to_user_id (app_id, user_id) ON DELETE CASCADE,
                                   CONSTRAINT st_app_id_to_user_id_app_id_fkey FOREIGN KEY (app_id) REFERENCES public.st_apps(app_id) ON DELETE CASCADE
);

CREATE INDEX st_app_id_to_user_id_app_id_index ON st_app_id_to_user_id (app_id);

CREATE INDEX st_app_id_to_user_id_primary_user_id_index ON st_app_id_to_user_id (primary_or_recipe_user_id, app_id);

CREATE TABLE st_all_auth_recipe_users (
                                       app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                       tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                       user_id character(36) NOT NULL,
                                       primary_or_recipe_user_id CHAR(36) NOT NULL,
                                       is_linked_or_is_a_primary_user BOOLEAN NOT NULL DEFAULT FALSE,
                                       recipe_id VARCHAR(128) NOT NULL,
                                       time_joined BIGINT NOT NULL,
                                       primary_or_recipe_user_time_joined BIGINT NOT NULL,
                                       CONSTRAINT st_all_auth_recipe_users_pkey PRIMARY KEY (app_id, tenant_id, user_id),
                                       CONSTRAINT st_all_auth_recipe_users_tenant_id_fkey FOREIGN KEY (app_id, tenant_id) REFERENCES public.st_tenants(app_id, tenant_id) ON DELETE CASCADE,
                                       CONSTRAINT st_all_auth_recipe_users_primary_or_recipe_user_id_fkey FOREIGN KEY(app_id, primary_or_recipe_user_id) REFERENCES public.st_app_id_to_user_id (app_id, user_id) ON DELETE CASCADE,
                                       CONSTRAINT st_all_auth_recipe_users_user_id_fkey FOREIGN KEY (app_id, user_id) REFERENCES public.st_app_id_to_user_id(app_id, user_id) ON DELETE CASCADE
);

CREATE INDEX st_all_auth_recipe_users_pagination_index1 ON st_all_auth_recipe_users
    (app_id, tenant_id, primary_or_recipe_user_time_joined DESC, primary_or_recipe_user_id DESC);

CREATE INDEX st_all_auth_recipe_users_pagination_index2 ON st_all_auth_recipe_users
    (app_id, tenant_id, primary_or_recipe_user_time_joined ASC, primary_or_recipe_user_id DESC);

CREATE INDEX st_all_auth_recipe_users_pagination_index3 ON st_all_auth_recipe_users
    (recipe_id, app_id, tenant_id, primary_or_recipe_user_time_joined DESC, primary_or_recipe_user_id DESC);


CREATE INDEX st_all_auth_recipe_users_pagination_index4 ON st_all_auth_recipe_users
    (recipe_id, app_id, tenant_id, primary_or_recipe_user_time_joined ASC, primary_or_recipe_user_id DESC);


CREATE INDEX st_all_auth_recipe_users_primary_user_id_index ON st_all_auth_recipe_users
    (primary_or_recipe_user_id, app_id);

CREATE INDEX st_all_auth_recipe_users_recipe_id_index ON st_all_auth_recipe_users
    (app_id, recipe_id, tenant_id);

CREATE INDEX all_auth_recipe_user_id_index ON st_all_auth_recipe_users (app_id, user_id);

CREATE INDEX all_auth_recipe_tenant_id_index ON st_all_auth_recipe_users (app_id, tenant_id);

CREATE TABLE st_userid_mapping (
                                app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                supertokens_user_id character(36) NOT NULL,
                                external_user_id VARCHAR(128) NOT NULL,
                                external_user_id_info TEXT,
                                CONSTRAINT st_userid_mapping_external_user_id_key UNIQUE (app_id, external_user_id),
                                CONSTRAINT st_userid_mapping_pkey PRIMARY KEY (app_id, supertokens_user_id, external_user_id),
                                CONSTRAINT st_userid_mapping_supertokens_user_id_key UNIQUE (app_id, supertokens_user_id),
                                CONSTRAINT st_userid_mapping_supertokens_user_id_fkey FOREIGN KEY (app_id, supertokens_user_id) REFERENCES public.st_app_id_to_user_id(app_id, user_id) ON DELETE CASCADE
);

CREATE INDEX st_userid_mapping_supertokens_user_id_index ON st_userid_mapping (app_id, supertokens_user_id);

CREATE TABLE st_dashboard_users (
                                 app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                 user_id character(36) NOT NULL,
                                 email VARCHAR(256) NOT NULL,
                                 password_hash VARCHAR(256) NOT NULL,
                                 time_joined BIGINT NOT NULL,
                                 CONSTRAINT st_dashboard_users_email_key UNIQUE (app_id, email),
                                 CONSTRAINT st_dashboard_users_pkey PRIMARY KEY (app_id, user_id),
                                 CONSTRAINT st_dashboard_users_app_id_fkey FOREIGN KEY (app_id) REFERENCES public.st_apps(app_id) ON DELETE CASCADE
);

CREATE INDEX st_dashboard_users_app_id_index ON st_dashboard_users (app_id);

CREATE TABLE st_dashboard_user_sessions (
                                         app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                         session_id character(36) NOT NULL,
                                         user_id character(36) NOT NULL,
                                         time_created BIGINT NOT NULL,
                                         expiry BIGINT NOT NULL,
                                         CONSTRAINT st_dashboard_user_sessions_pkey PRIMARY KEY (app_id, session_id),
                                         CONSTRAINT st_dashboard_user_sessions_user_id_fkey FOREIGN KEY (app_id, user_id) REFERENCES public.st_dashboard_users(app_id, user_id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE INDEX st_dashboard_user_sessions_expiry_index ON st_dashboard_user_sessions (expiry);

CREATE INDEX st_dashboard_user_sessions_user_id_index ON st_dashboard_user_sessions (app_id, user_id);

CREATE TABLE st_session_access_token_signing_keys (
                                                   app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                                   created_at_time BIGINT NOT NULL,
                                                   value TEXT,
                                                   CONSTRAINT st_session_access_token_signing_keys_pkey PRIMARY KEY (app_id, created_at_time),
                                                   CONSTRAINT st_session_access_token_signing_keys_app_id_fkey FOREIGN KEY (app_id) REFERENCES public.st_apps(app_id) ON DELETE CASCADE
);

CREATE INDEX access_token_signing_keys_app_id_index ON st_session_access_token_signing_keys (app_id);

CREATE TABLE st_session_info (
                              app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                              tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                              session_handle VARCHAR(255) NOT NULL,
                              user_id VARCHAR(128) NOT NULL,
                              refresh_token_hash_2 VARCHAR(128) NOT NULL,
                              session_data TEXT,
                              expires_at BIGINT NOT NULL,
                              created_at_time BIGINT NOT NULL,
                              jwt_user_payload TEXT,
                              use_static_key BOOLEAN NOT NULL,
                              CONSTRAINT st_session_info_pkey PRIMARY KEY (app_id, tenant_id, session_handle),
                              CONSTRAINT st_session_info_tenant_id_fkey FOREIGN KEY (app_id, tenant_id) REFERENCES public.st_tenants(app_id, tenant_id) ON DELETE CASCADE
);

CREATE INDEX session_expiry_index ON st_session_info (expires_at);

CREATE INDEX st_session_info_tenant_id_index ON st_session_info (app_id, tenant_id);

CREATE TABLE st_user_last_active (
                                  app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                  user_id VARCHAR(128) NOT NULL,
                                  last_active_time BIGINT,
                                  CONSTRAINT st_user_last_active_pkey PRIMARY KEY (app_id, user_id),
                                  CONSTRAINT st_user_last_active_app_id_fkey FOREIGN KEY (app_id) REFERENCES public.st_apps(app_id) ON DELETE CASCADE
);

CREATE INDEX st_user_last_active_app_id_index ON st_user_last_active (app_id);

CREATE TABLE st_emailpassword_users (
                                     app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                     user_id character(36) NOT NULL,
                                     email VARCHAR(256) NOT NULL,
                                     password_hash VARCHAR(256) NOT NULL,
                                     time_joined BIGINT NOT NULL,
                                     CONSTRAINT st_emailpassword_users_pkey PRIMARY KEY (app_id, user_id),
                                     CONSTRAINT st_emailpassword_users_user_id_fkey FOREIGN KEY (app_id, user_id) REFERENCES public.st_app_id_to_user_id(app_id, user_id) ON DELETE CASCADE
);

CREATE TABLE st_emailpassword_user_to_tenant (
                                              app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                              tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                              user_id character(36) NOT NULL,
                                              email VARCHAR(256) NOT NULL,
                                              CONSTRAINT st_emailpassword_user_to_tenant_email_key UNIQUE (app_id, tenant_id, email),
                                              CONSTRAINT st_emailpassword_user_to_tenant_pkey PRIMARY KEY (app_id, tenant_id, user_id),
                                              CONSTRAINT st_emailpassword_user_to_tenant_user_id_fkey FOREIGN KEY (app_id, tenant_id, user_id) REFERENCES public.st_all_auth_recipe_users(app_id, tenant_id, user_id) ON DELETE CASCADE
);

CREATE TABLE st_emailpassword_pswd_reset_tokens (
                                                 app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                                 user_id character(36) NOT NULL,
                                                 token VARCHAR(128) NOT NULL,
                                                 token_expiry BIGINT NOT NULL,
                                                 email VARCHAR(256),
                                                 CONSTRAINT st_emailpassword_pswd_reset_tokens_pkey PRIMARY KEY (app_id, user_id, token),
                                                 CONSTRAINT st_emailpassword_pswd_reset_tokens_token_key UNIQUE (token),
                                                 CONSTRAINT st_emailpassword_pswd_reset_tokens_user_id_fkey FOREIGN KEY (app_id, user_id) REFERENCES public.st_app_id_to_user_id(app_id, user_id) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE INDEX emailpassword_password_reset_token_expiry_index ON st_emailpassword_pswd_reset_tokens (token_expiry);

CREATE INDEX st_emailpassword_pswd_reset_tokens_user_id_index ON st_emailpassword_pswd_reset_tokens (app_id, user_id);

CREATE TABLE st_emailverification_verified_emails (
                                                   app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                                   user_id VARCHAR(128) NOT NULL,
                                                   email VARCHAR(256) NOT NULL,
                                                   CONSTRAINT st_emailverification_verified_emails_pkey PRIMARY KEY (app_id, user_id, email),
                                                   CONSTRAINT st_emailverification_verified_emails_app_id_fkey FOREIGN KEY (app_id) REFERENCES public.st_apps(app_id) ON DELETE CASCADE
);

CREATE INDEX st_emailverification_verified_emails_app_id_index ON st_emailverification_verified_emails (app_id);

CREATE TABLE st_emailverification_tokens (
                                          app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                          tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                          user_id VARCHAR(128) NOT NULL,
                                          email VARCHAR(256) NOT NULL,
                                          token VARCHAR(128) NOT NULL,
                                          token_expiry BIGINT NOT NULL,
                                          CONSTRAINT st_emailverification_tokens_pkey PRIMARY KEY (app_id, tenant_id, user_id, email, token),
                                          CONSTRAINT st_emailverification_tokens_token_key UNIQUE (token),
                                          CONSTRAINT st_emailverification_tokens_tenant_id_fkey FOREIGN KEY (app_id, tenant_id) REFERENCES public.st_tenants(app_id, tenant_id) ON DELETE CASCADE
);

CREATE INDEX st_emailverification_tokens_index ON st_emailverification_tokens (token_expiry);

CREATE INDEX st_emailverification_tokens_tenant_id_index ON st_emailverification_tokens (app_id, tenant_id);

CREATE TABLE st_thirdparty_users (
                                  app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                  third_party_id VARCHAR(28) NOT NULL,
                                  third_party_user_id VARCHAR(256) NOT NULL,
                                  user_id character(36) NOT NULL,
                                  email VARCHAR(256) NOT NULL,
                                  time_joined BIGINT NOT NULL,
                                  CONSTRAINT st_thirdparty_users_pkey PRIMARY KEY (app_id, user_id),
                                  CONSTRAINT st_thirdparty_users_user_id_fkey FOREIGN KEY (app_id, user_id) REFERENCES public.st_app_id_to_user_id(app_id, user_id) ON DELETE CASCADE
);

CREATE INDEX st_thirdparty_users_email_index ON st_thirdparty_users (app_id, email);

CREATE INDEX st_thirdparty_users_thirdparty_user_id_index ON st_thirdparty_users (app_id, third_party_id, third_party_user_id);

CREATE TABLE st_thirdparty_user_to_tenant (
                                           app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                           tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                           user_id character(36) NOT NULL,
                                           third_party_id VARCHAR(28) NOT NULL,
                                           third_party_user_id VARCHAR(256) NOT NULL,
                                           CONSTRAINT st_thirdparty_user_to_tenant_pkey PRIMARY KEY (app_id, tenant_id, user_id),
                                           CONSTRAINT st_thirdparty_user_to_tenant_third_party_user_id_key UNIQUE (app_id, tenant_id, third_party_id, third_party_user_id),
                                           CONSTRAINT st_thirdparty_user_to_tenant_user_id_fkey FOREIGN KEY (app_id, tenant_id, user_id) REFERENCES public.st_all_auth_recipe_users(app_id, tenant_id, user_id) ON DELETE CASCADE
);

CREATE TABLE st_passwordless_users (
                                    app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                    user_id character(36) NOT NULL,
                                    email VARCHAR(256),
                                    phone_number VARCHAR(256),
                                    time_joined BIGINT NOT NULL,
                                    CONSTRAINT st_passwordless_users_pkey PRIMARY KEY (app_id, user_id),
                                    CONSTRAINT st_passwordless_users_user_id_fkey FOREIGN KEY (app_id, user_id) REFERENCES public.st_app_id_to_user_id(app_id, user_id) ON DELETE CASCADE
);

CREATE TABLE st_passwordless_user_to_tenant (
                                             app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                             tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                             user_id character(36) NOT NULL,
                                             email VARCHAR(256),
                                             phone_number VARCHAR(256),
                                             CONSTRAINT st_passwordless_user_to_tenant_email_key UNIQUE (app_id, tenant_id, email),
                                             CONSTRAINT st_passwordless_user_to_tenant_phone_number_key UNIQUE (app_id, tenant_id, phone_number),
                                             CONSTRAINT st_passwordless_user_to_tenant_pkey PRIMARY KEY (app_id, tenant_id, user_id),
                                             CONSTRAINT st_passwordless_user_to_tenant_user_id_fkey FOREIGN KEY (app_id, tenant_id, user_id) REFERENCES public.st_all_auth_recipe_users(app_id, tenant_id, user_id) ON DELETE CASCADE
);

CREATE TABLE st_passwordless_devices (
                                      app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                      tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                      device_id_hash character(44) NOT NULL,
                                      email VARCHAR(256),
                                      phone_number VARCHAR(256),
                                      link_code_salt character(44) NOT NULL,
                                      failed_attempts integer NOT NULL,
                                      CONSTRAINT st_passwordless_devices_pkey PRIMARY KEY (app_id, tenant_id, device_id_hash),
                                      CONSTRAINT st_passwordless_devices_tenant_id_fkey FOREIGN KEY (app_id, tenant_id) REFERENCES public.st_tenants(app_id, tenant_id) ON DELETE CASCADE
);

CREATE INDEX st_passwordless_devices_email_index ON st_passwordless_devices (app_id, tenant_id, email);

CREATE INDEX st_passwordless_devices_phone_number_index ON st_passwordless_devices (app_id, tenant_id, phone_number);

CREATE INDEX st_passwordless_devices_tenant_id_index ON st_passwordless_devices (app_id, tenant_id);

CREATE TABLE st_passwordless_codes (
                                    app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                    tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                    code_id character(36) NOT NULL,
                                    device_id_hash character(44) NOT NULL,
                                    link_code_hash character(44) NOT NULL,
                                    created_at BIGINT NOT NULL,
                                    CONSTRAINT st_passwordless_codes_link_code_hash_key UNIQUE (app_id, tenant_id, link_code_hash),
                                    CONSTRAINT st_passwordless_codes_pkey PRIMARY KEY (app_id, tenant_id, code_id),
                                    CONSTRAINT st_passwordless_codes_device_id_hash_fkey FOREIGN KEY (app_id, tenant_id, device_id_hash) REFERENCES public.st_passwordless_devices(app_id, tenant_id, device_id_hash) ON UPDATE CASCADE ON DELETE CASCADE
);

CREATE INDEX st_passwordless_codes_created_at_index ON st_passwordless_codes (app_id, tenant_id, created_at);

CREATE INDEX st_passwordless_codes_device_id_hash_index ON st_passwordless_codes (app_id, tenant_id, device_id_hash);

CREATE TABLE st_jwt_signing_keys (
                                  app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                  key_id VARCHAR(255) NOT NULL,
                                  key_string TEXT NOT NULL,
                                  algorithm VARCHAR(10) NOT NULL,
                                  created_at BIGINT,
                                  CONSTRAINT st_jwt_signing_keys_pkey PRIMARY KEY (app_id, key_id),
                                  CONSTRAINT st_jwt_signing_keys_app_id_fkey FOREIGN KEY (app_id) REFERENCES public.st_apps(app_id) ON DELETE CASCADE
);

CREATE INDEX st_jwt_signing_keys_app_id_index ON st_jwt_signing_keys (app_id);

CREATE TABLE st_user_metadata (
                               app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                               user_id VARCHAR(128) NOT NULL,
                               st_user_metadata TEXT NOT NULL,
                               CONSTRAINT st_user_metadata_pkey PRIMARY KEY (app_id, user_id),
                               CONSTRAINT st_user_metadata_app_id_fkey FOREIGN KEY (app_id) REFERENCES public.st_apps(app_id) ON DELETE CASCADE
);

CREATE INDEX st_user_metadata_app_id_index ON st_user_metadata (app_id);

CREATE TABLE st_roles (
                       app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                       role VARCHAR(255) NOT NULL,
                       CONSTRAINT st_roles_pkey PRIMARY KEY (app_id, role),
                       CONSTRAINT st_roles_app_id_fkey FOREIGN KEY (app_id) REFERENCES public.st_apps(app_id) ON DELETE CASCADE
);

CREATE INDEX st_roles_app_id_index ON st_roles (app_id);

CREATE TABLE st_role_permissions (
                                  app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                  role VARCHAR(255) NOT NULL,
                                  permission VARCHAR(255) NOT NULL,
                                  CONSTRAINT st_role_permissions_pkey PRIMARY KEY (app_id, role, permission),
                                  CONSTRAINT st_role_permissions_role_fkey FOREIGN KEY (app_id, role) REFERENCES public.st_roles(app_id, role) ON DELETE CASCADE
);

CREATE INDEX st_role_permissions_permission_index ON st_role_permissions (app_id, permission);

CREATE INDEX st_role_permissions_role_index ON st_role_permissions (app_id, role);

CREATE TABLE st_user_st_roles (
                            app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                            tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                            user_id VARCHAR(128) NOT NULL,
                            role VARCHAR(255) NOT NULL,
                            CONSTRAINT st_user_st_roles_pkey PRIMARY KEY (app_id, tenant_id, user_id, role),
                            CONSTRAINT st_user_st_roles_tenant_id_fkey FOREIGN KEY (app_id, tenant_id) REFERENCES public.st_tenants(app_id, tenant_id) ON DELETE CASCADE
);

CREATE INDEX st_user_st_roles_role_index ON st_user_st_roles (app_id, tenant_id, role);

CREATE INDEX st_user_st_roles_tenant_id_index ON st_user_st_roles (app_id, tenant_id);

CREATE INDEX st_user_st_roles_app_id_role_index ON st_user_st_roles (app_id, role);

CREATE TABLE st_totp_users (
                            app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                            user_id VARCHAR(128) NOT NULL,
                            CONSTRAINT st_totp_users_pkey PRIMARY KEY (app_id, user_id),
                            CONSTRAINT st_totp_users_app_id_fkey FOREIGN KEY (app_id) REFERENCES public.st_apps(app_id) ON DELETE CASCADE
);

CREATE INDEX st_totp_users_app_id_index ON st_totp_users (app_id);

CREATE TABLE st_totp_user_devices (
                                   app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                   user_id VARCHAR(128) NOT NULL,
                                   device_name VARCHAR(256) NOT NULL,
                                   secret_key VARCHAR(256) NOT NULL,
                                   period integer NOT NULL,
                                   skew integer NOT NULL,
                                   verified BOOLEAN NOT NULL,
                                   created_at BIGINT,
                                   CONSTRAINT st_totp_user_devices_pkey PRIMARY KEY (app_id, user_id, device_name),
                                   CONSTRAINT st_totp_user_devices_user_id_fkey FOREIGN KEY (app_id, user_id) REFERENCES public.st_totp_users(app_id, user_id) ON DELETE CASCADE
);

CREATE INDEX st_totp_user_devices_user_id_index ON st_totp_user_devices (app_id, user_id);

CREATE TABLE st_totp_used_codes (
                                 app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                 tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                 user_id VARCHAR(128) NOT NULL,
                                 code VARCHAR(8) NOT NULL,
                                 is_valid BOOLEAN NOT NULL,
                                 expiry_time_ms BIGINT NOT NULL,
                                 created_time_ms BIGINT NOT NULL,
                                 CONSTRAINT st_totp_used_codes_pkey PRIMARY KEY (app_id, tenant_id, user_id, created_time_ms),
                                 CONSTRAINT st_totp_used_codes_tenant_id_fkey FOREIGN KEY (app_id, tenant_id) REFERENCES public.st_tenants(app_id, tenant_id) ON DELETE CASCADE,
                                 CONSTRAINT st_totp_used_codes_user_id_fkey FOREIGN KEY (app_id, user_id) REFERENCES public.st_totp_users(app_id, user_id) ON DELETE CASCADE
);

CREATE INDEX st_totp_used_codes_expiry_time_ms_index ON st_totp_used_codes (app_id, tenant_id, expiry_time_ms);

CREATE INDEX st_totp_used_codes_tenant_id_index ON st_totp_used_codes (app_id, tenant_id);

CREATE INDEX st_totp_used_codes_user_id_index ON st_totp_used_codes (app_id, user_id);

CREATE TABLE IF NOT EXISTS st_bulk_import_users (
                                                 id CHAR(36),
    app_id VARCHAR(64) NOT NULL DEFAULT 'public',
    primary_user_id VARCHAR(36),
    raw_data TEXT NOT NULL,
    status VARCHAR(128) DEFAULT 'NEW',
    error_msg TEXT,
    created_at BIGINT NOT NULL,
    updated_at BIGINT NOT NULL,
    CONSTRAINT st_bulk_import_users_pkey PRIMARY KEY(app_id, id),
    CONSTRAINT st_bulk_import_users__app_id_fkey FOREIGN KEY(app_id) REFERENCES st_apps(app_id) ON DELETE CASCADE
    );

CREATE INDEX IF NOT EXISTS st_bulk_import_users_status_updated_at_index ON st_bulk_import_users (app_id, status, updated_at);

CREATE INDEX IF NOT EXISTS st_bulk_import_users_pagination_index1 ON st_bulk_import_users (app_id, status, created_at DESC, id DESC);

CREATE INDEX IF NOT EXISTS st_bulk_import_users_pagination_index2 ON st_bulk_import_users (app_id, created_at DESC, id DESC);

CREATE INDEX IF NOT EXISTS st_session_info_user_id_app_id_index ON st_session_info (user_id, app_id);