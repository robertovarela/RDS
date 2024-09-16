using System;
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
            migrationBuilder.CreateTable(
                name: "CompanyUserRegistration",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<long>(type: "BIGINT", nullable: false),
                    CompanyName = table.Column<string>(type: "NVARCHAR(80)", maxLength: 80, nullable: false),
                    OwnerId = table.Column<long>(type: "BIGINT", nullable: false),
                    Email = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    ConfirmationCode = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "DATETIME", nullable: false),
                    ConfirmationDate = table.Column<DateTime>(type: "DATETIME", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyUserRegistration", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyUserRegistration_CompanyId_Email",
                table: "CompanyUserRegistration",
                columns: new[] { "CompanyId", "Email" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyUserRegistration");
        }
    }
}
