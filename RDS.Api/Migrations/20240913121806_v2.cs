using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDS.Api.Migrations
{
    /// <inheritdoc />
    public partial class v2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "IdentityRole");

            migrationBuilder.DropColumn(
                name: "RoleName",
                table: "IdentityRole");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "IdentityRole",
                type: "BIGINT",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Relational:ColumnOrder", 5);

            migrationBuilder.AddColumn<string>(
                name: "RoleName",
                table: "IdentityRole",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
