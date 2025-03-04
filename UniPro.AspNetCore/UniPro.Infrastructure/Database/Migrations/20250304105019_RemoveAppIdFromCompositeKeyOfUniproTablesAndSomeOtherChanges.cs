using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniPro.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAppIdFromCompositeKeyOfUniproTablesAndSomeOtherChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_grades_student_tasks_app_id_student_id_task_id",
                table: "grades");

            migrationBuilder.DropForeignKey(
                name: "fk_student_infos_users_app_id_student_id",
                table: "student_infos");

            migrationBuilder.DropForeignKey(
                name: "fk_student_tasks_student_infos_app_id_student_id",
                table: "student_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_tasks_teacher_infos_app_id_teacher_id",
                table: "tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_teacher_infos_users_app_id_teacher_id",
                table: "teacher_infos");

            migrationBuilder.DropPrimaryKey(
                name: "users_pkey",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_teacher_infos",
                table: "teacher_infos");

            migrationBuilder.DropIndex(
                name: "ix_tasks_app_id_teacher_id",
                table: "tasks");

            migrationBuilder.DropPrimaryKey(
                name: "pk_student_tasks",
                table: "student_tasks");

            migrationBuilder.DropPrimaryKey(
                name: "pk_student_infos",
                table: "student_infos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_grades",
                table: "grades");

            migrationBuilder.DropColumn(
                name: "app_id",
                table: "teacher_infos");

            migrationBuilder.DropColumn(
                name: "app_id",
                table: "tasks");

            migrationBuilder.DropColumn(
                name: "app_id",
                table: "student_tasks");

            migrationBuilder.DropColumn(
                name: "app_id",
                table: "student_infos");

            migrationBuilder.DropColumn(
                name: "app_id",
                table: "grades");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<string>(
                name: "avatar",
                table: "users",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "tasks",
                type: "character varying(128)",
                maxLength: 128,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddPrimaryKey(
                name: "users_pkey",
                table: "users",
                column: "user_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_teacher_infos",
                table: "teacher_infos",
                column: "teacher_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_student_tasks",
                table: "student_tasks",
                columns: new[] { "student_id", "task_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_student_infos",
                table: "student_infos",
                column: "student_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_grades",
                table: "grades",
                columns: new[] { "student_id", "task_id" });

            migrationBuilder.CreateIndex(
                name: "ix_users_app_id_user_id",
                table: "users",
                columns: new[] { "app_id", "user_id" });

            migrationBuilder.CreateIndex(
                name: "ix_tasks_teacher_id",
                table: "tasks",
                column: "teacher_id");

            migrationBuilder.AddForeignKey(
                name: "fk_grades_student_tasks_student_id_task_id",
                table: "grades",
                columns: new[] { "student_id", "task_id" },
                principalTable: "student_tasks",
                principalColumns: new[] { "student_id", "task_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_student_infos_users_student_id",
                table: "student_infos",
                column: "student_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_student_tasks_student_infos_student_id",
                table: "student_tasks",
                column: "student_id",
                principalTable: "student_infos",
                principalColumn: "student_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_tasks_teacher_infos_teacher_id",
                table: "tasks",
                column: "teacher_id",
                principalTable: "teacher_infos",
                principalColumn: "teacher_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_teacher_infos_users_teacher_id",
                table: "teacher_infos",
                column: "teacher_id",
                principalTable: "users",
                principalColumn: "user_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_grades_student_tasks_student_id_task_id",
                table: "grades");

            migrationBuilder.DropForeignKey(
                name: "fk_student_infos_users_student_id",
                table: "student_infos");

            migrationBuilder.DropForeignKey(
                name: "fk_student_tasks_student_infos_student_id",
                table: "student_tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_tasks_teacher_infos_teacher_id",
                table: "tasks");

            migrationBuilder.DropForeignKey(
                name: "fk_teacher_infos_users_teacher_id",
                table: "teacher_infos");

            migrationBuilder.DropPrimaryKey(
                name: "users_pkey",
                table: "users");

            migrationBuilder.DropIndex(
                name: "ix_users_app_id_user_id",
                table: "users");

            migrationBuilder.DropPrimaryKey(
                name: "pk_teacher_infos",
                table: "teacher_infos");

            migrationBuilder.DropIndex(
                name: "ix_tasks_teacher_id",
                table: "tasks");

            migrationBuilder.DropPrimaryKey(
                name: "pk_student_tasks",
                table: "student_tasks");

            migrationBuilder.DropPrimaryKey(
                name: "pk_student_infos",
                table: "student_infos");

            migrationBuilder.DropPrimaryKey(
                name: "pk_grades",
                table: "grades");

            migrationBuilder.AlterColumn<DateTime>(
                name: "updated_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "avatar",
                table: "users",
                type: "character varying(1024)",
                maxLength: 1024,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(1024)",
                oldMaxLength: 1024,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "app_id",
                table: "teacher_infos",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValueSql: "'public'::character varying");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "tasks",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(128)",
                oldMaxLength: 128);

            migrationBuilder.AddColumn<string>(
                name: "app_id",
                table: "tasks",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValueSql: "'public'::character varying");

            migrationBuilder.AddColumn<string>(
                name: "app_id",
                table: "student_tasks",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValueSql: "'public'::character varying");

            migrationBuilder.AddColumn<string>(
                name: "app_id",
                table: "student_infos",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValueSql: "'public'::character varying");

            migrationBuilder.AddColumn<string>(
                name: "app_id",
                table: "grades",
                type: "character varying(64)",
                maxLength: 64,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "users_pkey",
                table: "users",
                columns: new[] { "app_id", "user_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_teacher_infos",
                table: "teacher_infos",
                columns: new[] { "app_id", "teacher_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_student_tasks",
                table: "student_tasks",
                columns: new[] { "app_id", "student_id", "task_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_student_infos",
                table: "student_infos",
                columns: new[] { "app_id", "student_id" });

            migrationBuilder.AddPrimaryKey(
                name: "pk_grades",
                table: "grades",
                columns: new[] { "app_id", "student_id", "task_id" });

            migrationBuilder.CreateIndex(
                name: "ix_tasks_app_id_teacher_id",
                table: "tasks",
                columns: new[] { "app_id", "teacher_id" });

            migrationBuilder.AddForeignKey(
                name: "fk_grades_student_tasks_app_id_student_id_task_id",
                table: "grades",
                columns: new[] { "app_id", "student_id", "task_id" },
                principalTable: "student_tasks",
                principalColumns: new[] { "app_id", "student_id", "task_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_student_infos_users_app_id_student_id",
                table: "student_infos",
                columns: new[] { "app_id", "student_id" },
                principalTable: "users",
                principalColumns: new[] { "app_id", "user_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_student_tasks_student_infos_app_id_student_id",
                table: "student_tasks",
                columns: new[] { "app_id", "student_id" },
                principalTable: "student_infos",
                principalColumns: new[] { "app_id", "student_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_tasks_teacher_infos_app_id_teacher_id",
                table: "tasks",
                columns: new[] { "app_id", "teacher_id" },
                principalTable: "teacher_infos",
                principalColumns: new[] { "app_id", "teacher_id" },
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_teacher_infos_users_app_id_teacher_id",
                table: "teacher_infos",
                columns: new[] { "app_id", "teacher_id" },
                principalTable: "users",
                principalColumns: new[] { "app_id", "user_id" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
