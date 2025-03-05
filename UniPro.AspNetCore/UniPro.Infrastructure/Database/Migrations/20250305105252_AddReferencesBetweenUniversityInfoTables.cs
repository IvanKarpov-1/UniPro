using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniPro.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddReferencesBetweenUniversityInfoTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "department_id",
                table: "student_groups",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "academic_id",
                table: "departments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "university_id",
                table: "academics",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_student_groups_department_id",
                table: "student_groups",
                column: "department_id");

            migrationBuilder.CreateIndex(
                name: "ix_departments_academic_id",
                table: "departments",
                column: "academic_id");

            migrationBuilder.CreateIndex(
                name: "ix_academics_university_id",
                table: "academics",
                column: "university_id");

            migrationBuilder.AddForeignKey(
                name: "fk_academics_universities_university_id",
                table: "academics",
                column: "university_id",
                principalTable: "universities",
                principalColumn: "university_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_departments_academics_academic_id",
                table: "departments",
                column: "academic_id",
                principalTable: "academics",
                principalColumn: "academic_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_student_groups_departments_department_id",
                table: "student_groups",
                column: "department_id",
                principalTable: "departments",
                principalColumn: "department_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_academics_universities_university_id",
                table: "academics");

            migrationBuilder.DropForeignKey(
                name: "fk_departments_academics_academic_id",
                table: "departments");

            migrationBuilder.DropForeignKey(
                name: "fk_student_groups_departments_department_id",
                table: "student_groups");

            migrationBuilder.DropIndex(
                name: "ix_student_groups_department_id",
                table: "student_groups");

            migrationBuilder.DropIndex(
                name: "ix_departments_academic_id",
                table: "departments");

            migrationBuilder.DropIndex(
                name: "ix_academics_university_id",
                table: "academics");

            migrationBuilder.DropColumn(
                name: "department_id",
                table: "student_groups");

            migrationBuilder.DropColumn(
                name: "academic_id",
                table: "departments");

            migrationBuilder.DropColumn(
                name: "university_id",
                table: "academics");
        }
    }
}
