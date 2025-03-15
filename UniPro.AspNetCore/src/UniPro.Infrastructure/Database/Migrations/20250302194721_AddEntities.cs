using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace UniPro.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "phone_number",
                table: "users",
                type: "character varying(36)",
                maxLength: 36,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "academics",
                columns: table => new
                {
                    academic_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_academics", x => x.academic_id);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    course_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    course_name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    credits = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_courses", x => x.course_id);
                });

            migrationBuilder.CreateTable(
                name: "departments",
                columns: table => new
                {
                    department_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_departments", x => x.department_id);
                });

            migrationBuilder.CreateTable(
                name: "student_groups",
                columns: table => new
                {
                    student_group_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_student_groups", x => x.student_group_id);
                });

            migrationBuilder.CreateTable(
                name: "task_types",
                columns: table => new
                {
                    task_type_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_task_types", x => x.task_type_id);
                });

            migrationBuilder.CreateTable(
                name: "universities",
                columns: table => new
                {
                    university_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_universities", x => x.university_id);
                });

            migrationBuilder.CreateTable(
                name: "student_infos",
                columns: table => new
                {
                    app_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, defaultValueSql: "'public'::character varying"),
                    student_id = table.Column<string>(type: "character(36)", fixedLength: true, maxLength: 36, nullable: false),
                    student_group_id = table.Column<int>(type: "integer", nullable: false),
                    department_id = table.Column<int>(type: "integer", nullable: false),
                    academic_id = table.Column<int>(type: "integer", nullable: false),
                    university_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_student_infos", x => new { x.app_id, x.student_id });
                    table.ForeignKey(
                        name: "fk_student_infos_academics_academic_id",
                        column: x => x.academic_id,
                        principalTable: "academics",
                        principalColumn: "academic_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_student_infos_departments_department_id",
                        column: x => x.department_id,
                        principalTable: "departments",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_student_infos_student_groups_student_group_id",
                        column: x => x.student_group_id,
                        principalTable: "student_groups",
                        principalColumn: "student_group_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_student_infos_universities_university_id",
                        column: x => x.university_id,
                        principalTable: "universities",
                        principalColumn: "university_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_student_infos_users_app_id_student_id",
                        columns: x => new { x.app_id, x.student_id },
                        principalTable: "users",
                        principalColumns: new[] { "app_id", "user_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "teacher_infos",
                columns: table => new
                {
                    app_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, defaultValueSql: "'public'::character varying"),
                    teacher_id = table.Column<string>(type: "character(36)", fixedLength: true, maxLength: 36, nullable: false),
                    department_id = table.Column<int>(type: "integer", nullable: false),
                    academic_id = table.Column<int>(type: "integer", nullable: false),
                    university_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_teacher_infos", x => new { x.app_id, x.teacher_id });
                    table.ForeignKey(
                        name: "fk_teacher_infos_academics_academic_id",
                        column: x => x.academic_id,
                        principalTable: "academics",
                        principalColumn: "academic_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teacher_infos_departments_department_id",
                        column: x => x.department_id,
                        principalTable: "departments",
                        principalColumn: "department_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teacher_infos_universities_university_id",
                        column: x => x.university_id,
                        principalTable: "universities",
                        principalColumn: "university_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_teacher_infos_users_app_id_teacher_id",
                        columns: x => new { x.app_id, x.teacher_id },
                        principalTable: "users",
                        principalColumns: new[] { "app_id", "user_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                columns: table => new
                {
                    task_id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    course_id = table.Column<long>(type: "bigint", nullable: false),
                    task_type_id = table.Column<int>(type: "integer", nullable: false),
                    app_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, defaultValueSql: "'public'::character varying"),
                    teacher_id = table.Column<string>(type: "character(36)", fixedLength: true, maxLength: 36, nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    due_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tasks", x => x.task_id);
                    table.ForeignKey(
                        name: "fk_tasks_courses_course_id",
                        column: x => x.course_id,
                        principalTable: "courses",
                        principalColumn: "course_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tasks_task_types_task_type_id",
                        column: x => x.task_type_id,
                        principalTable: "task_types",
                        principalColumn: "task_type_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tasks_teacher_infos_app_id_teacher_id",
                        columns: x => new { x.app_id, x.teacher_id },
                        principalTable: "teacher_infos",
                        principalColumns: new[] { "app_id", "teacher_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "student_tasks",
                columns: table => new
                {
                    app_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false, defaultValueSql: "'public'::character varying"),
                    student_id = table.Column<string>(type: "character(36)", fixedLength: true, maxLength: 36, nullable: false),
                    task_id = table.Column<long>(type: "bigint", nullable: false),
                    task_type_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_student_tasks", x => new { x.app_id, x.student_id, x.task_id });
                    table.ForeignKey(
                        name: "fk_student_tasks_student_infos_app_id_student_id",
                        columns: x => new { x.app_id, x.student_id },
                        principalTable: "student_infos",
                        principalColumns: new[] { "app_id", "student_id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_student_tasks_tasks_task_type_id",
                        column: x => x.task_type_id,
                        principalTable: "tasks",
                        principalColumn: "task_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "grades",
                columns: table => new
                {
                    app_id = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    student_id = table.Column<string>(type: "character(36)", maxLength: 36, nullable: false),
                    task_id = table.Column<long>(type: "bigint", nullable: false),
                    grade_value = table.Column<float>(type: "real", nullable: false),
                    is_agreed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_grades", x => new { x.app_id, x.student_id, x.task_id });
                    table.ForeignKey(
                        name: "fk_grades_student_tasks_app_id_student_id_task_id",
                        columns: x => new { x.app_id, x.student_id, x.task_id },
                        principalTable: "student_tasks",
                        principalColumns: new[] { "app_id", "student_id", "task_id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_student_infos_academic_id",
                table: "student_infos",
                column: "academic_id");

            migrationBuilder.CreateIndex(
                name: "ix_student_infos_department_id",
                table: "student_infos",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "ix_student_infos_student_group_id",
                table: "student_infos",
                column: "student_group_id");

            migrationBuilder.CreateIndex(
                name: "ix_student_infos_university_id",
                table: "student_infos",
                column: "university_id");

            migrationBuilder.CreateIndex(
                name: "ix_student_tasks_task_type_id",
                table: "student_tasks",
                column: "task_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasks_app_id_teacher_id",
                table: "tasks",
                columns: new[] { "app_id", "teacher_id" });

            migrationBuilder.CreateIndex(
                name: "ix_tasks_course_id",
                table: "tasks",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "ix_tasks_task_type_id",
                table: "tasks",
                column: "task_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_teacher_infos_academic_id",
                table: "teacher_infos",
                column: "academic_id");

            migrationBuilder.CreateIndex(
                name: "ix_teacher_infos_department_id",
                table: "teacher_infos",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "ix_teacher_infos_university_id",
                table: "teacher_infos",
                column: "university_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "grades");

            migrationBuilder.DropTable(
                name: "student_tasks");

            migrationBuilder.DropTable(
                name: "student_infos");

            migrationBuilder.DropTable(
                name: "tasks");

            migrationBuilder.DropTable(
                name: "student_groups");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "task_types");

            migrationBuilder.DropTable(
                name: "teacher_infos");

            migrationBuilder.DropTable(
                name: "academics");

            migrationBuilder.DropTable(
                name: "departments");

            migrationBuilder.DropTable(
                name: "universities");

            migrationBuilder.DropColumn(
                name: "phone_number",
                table: "users");
        }
    }
}
