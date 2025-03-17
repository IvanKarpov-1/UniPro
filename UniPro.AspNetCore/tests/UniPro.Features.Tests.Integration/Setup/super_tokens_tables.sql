CREATE TABLE st_app_id_to_user_id (
                                   app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                                   user_id character(36) NOT NULL,
                                   recipe_id VARCHAR(128) NOT NULL,
                                   primary_or_recipe_user_id CHAR(36) NOT NULL,
                                   is_linked_or_is_a_primary_user BOOLEAN NOT NULL DEFAULT FALSE,
                                   CONSTRAINT st_app_id_to_user_id_pkey PRIMARY KEY (app_id, user_id),
                                   CONSTRAINT st_app_id_to_user_id_primary_or_recipe_user_id_fkey FOREIGN KEY(app_id, primary_or_recipe_user_id) REFERENCES st_app_id_to_user_id (app_id, user_id) ON DELETE CASCADE
);

CREATE TABLE st_roles (
                       app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                       role VARCHAR(255) NOT NULL,
                       CONSTRAINT st_roles_pkey PRIMARY KEY (app_id, role)
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

CREATE TABLE st_user_roles (
                            app_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                            tenant_id VARCHAR(64) DEFAULT 'public' NOT NULL,
                            user_id VARCHAR(128) NOT NULL,
                            role VARCHAR(255) NOT NULL,
                            CONSTRAINT st_user_roles_pkey PRIMARY KEY (app_id, tenant_id, user_id, role)
);

CREATE INDEX st_user_roles_role_index ON st_user_roles (app_id, tenant_id, role);

CREATE INDEX st_user_roles_tenant_id_index ON st_user_roles (app_id, tenant_id);

CREATE INDEX st_user_roles_app_id_role_index ON st_user_roles (app_id, role);