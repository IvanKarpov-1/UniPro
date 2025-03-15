using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniPro.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    app_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, defaultValueSql: "'public'::character varying"),
                    user_id = table.Column<string>(type: "character(36)", fixedLength: true, maxLength: 36, nullable: false),
                    first_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    lst_name = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    patronymic = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    avatar = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => new { x.app_id, x.user_id });
                    table.ForeignKey(
                        name: "st_users_user_id_fkey",
                        columns: x => new { x.app_id, x.user_id },
                        principalTable: "st_app_id_to_user_id",
                        principalColumns: new[] { "app_id", "user_id" },
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
