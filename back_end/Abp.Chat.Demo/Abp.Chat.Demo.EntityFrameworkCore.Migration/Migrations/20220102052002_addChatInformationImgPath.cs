using Microsoft.EntityFrameworkCore.Migrations;

namespace Abp.Chat.Demo.EntityFrameworkCore.Migrations.Migrations
{
    public partial class addChatInformationImgPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Path",
                table: "ChatInformation",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Path",
                table: "ChatInformation");
        }
    }
}
