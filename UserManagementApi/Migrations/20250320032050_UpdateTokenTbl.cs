using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserManagementApi.Migrations
{
    /// <inheritdoc />
    public partial class UpdateTokenTbl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Expiration",
                table: "AuthenticationTokens",
                newName: "RefreshTokenExpiration");

            migrationBuilder.AddColumn<DateTime>(
                name: "AuthTokenExpiration",
                table: "AuthenticationTokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuthTokenExpiration",
                table: "AuthenticationTokens");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpiration",
                table: "AuthenticationTokens",
                newName: "Expiration");
        }
    }
}
