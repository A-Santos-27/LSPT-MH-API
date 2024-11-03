using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LSPT_MH.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "URLs",
                columns: table => new
                {
                    OriginalUrl = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ShortUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_URLs", x => x.OriginalUrl);
                });

            migrationBuilder.CreateIndex(
                name: "IX_URLs_OriginalUrl",
                table: "URLs",
                column: "OriginalUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "URLs");
        }
    }
}
