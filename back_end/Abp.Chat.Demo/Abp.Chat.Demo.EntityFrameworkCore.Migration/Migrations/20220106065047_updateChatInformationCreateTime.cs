using Microsoft.EntityFrameworkCore.Migrations;

namespace Abp.Chat.Demo.EntityFrameworkCore.Migrations.Migrations
{
    public partial class updateChatInformationCreateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Created",
                table: "ChatInformation",
                newName: "CreatedTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedTime",
                table: "ChatInformation",
                newName: "Created");
        }
    }
}
