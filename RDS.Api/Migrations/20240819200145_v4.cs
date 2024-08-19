using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RDS.Api.Migrations
{
    /// <inheritdoc />
    public partial class v4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Transaction",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Order",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Category",
                newName: "CompanyId");

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(80)", maxLength: 80, nullable: false),
                    Description = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: true),
                    UserId = table.Column<long>(type: "BIGINT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Transaction",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Order",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "Category",
                newName: "UserId");
        }
    }
}
