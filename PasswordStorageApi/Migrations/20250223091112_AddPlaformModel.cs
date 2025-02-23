using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PasswordStorageApi.Migrations
{
    /// <inheritdoc />
    public partial class AddPlaformModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsCompromised",
                table: "Passwords");

            migrationBuilder.AddColumn<int>(
                name: "PlatformId",
                table: "Passwords",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Platforms",
                columns: table => new
                {
                    PlatformId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlatformName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platforms", x => x.PlatformId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Passwords_PlatformId",
                table: "Passwords",
                column: "PlatformId");

            migrationBuilder.AddForeignKey(
                name: "FK_Passwords_Platforms_PlatformId",
                table: "Passwords",
                column: "PlatformId",
                principalTable: "Platforms",
                principalColumn: "PlatformId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passwords_Platforms_PlatformId",
                table: "Passwords");

            migrationBuilder.DropTable(
                name: "Platforms");

            migrationBuilder.DropIndex(
                name: "IX_Passwords_PlatformId",
                table: "Passwords");

            migrationBuilder.DropColumn(
                name: "PlatformId",
                table: "Passwords");

            migrationBuilder.AddColumn<bool>(
                name: "IsCompromised",
                table: "Passwords",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
