using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordStorageApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPropPassModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsExpired",
                table: "Passwords",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsExpired",
                table: "Passwords");
        }
    }
}
