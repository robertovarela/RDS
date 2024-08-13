using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDS.Api.Migrations
{
    /// <inheritdoc />
    public partial class v3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ApplicationUserId",
                table: "IdentityUserRole",
                type: "BIGINT",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_IdentityUserRole_ApplicationUserId",
                table: "IdentityUserRole",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_IdentityUserRole_IdentityUser_ApplicationUserId",
                table: "IdentityUserRole",
                column: "ApplicationUserId",
                principalTable: "IdentityUser",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_IdentityUserRole_IdentityUser_ApplicationUserId",
                table: "IdentityUserRole");

            migrationBuilder.DropIndex(
                name: "IX_IdentityUserRole_ApplicationUserId",
                table: "IdentityUserRole");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "IdentityUserRole");
        }
    }
}
