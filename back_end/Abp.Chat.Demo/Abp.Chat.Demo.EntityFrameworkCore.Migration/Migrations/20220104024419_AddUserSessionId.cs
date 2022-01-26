using Microsoft.EntityFrameworkCore.Migrations;

namespace Abp.Chat.Demo.EntityFrameworkCore.Migrations.Migrations
{
    public partial class AddUserSessionId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SessionId",
                table: "User",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionId",
                table: "User");
        }
    }
}
