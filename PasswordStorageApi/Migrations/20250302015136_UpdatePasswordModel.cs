using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordStorageApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePasswordModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCurrent",
                table: "Passwords",
                newName: "IsActive");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "Passwords",
                newName: "IsCurrent");
        }
    }
}
