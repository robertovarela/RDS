using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDS.Api.Migrations
{
    /// <inheritdoc />
    public partial class v1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IdentityUser_Name",
                table: "IdentityUser");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "Address",
                type: "NVARCHAR(9)",
                maxLength: 9,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(8)",
                oldMaxLength: 8);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_Name",
                table: "IdentityUser",
                column: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_IdentityUser_Name",
                table: "IdentityUser");

            migrationBuilder.AlterColumn<string>(
                name: "PostalCode",
                table: "Address",
                type: "NVARCHAR(8)",
                maxLength: 8,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "NVARCHAR(9)",
                oldMaxLength: 9);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUser_Name",
                table: "IdentityUser",
                column: "Name",
                unique: true);
        }
    }
}
