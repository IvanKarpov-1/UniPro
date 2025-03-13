using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniPro.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class CorrectUsersLastNameColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "lst_name",
                table: "users",
                newName: "last_name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "last_name",
                table: "users",
                newName: "lst_name");
        }
    }
}
