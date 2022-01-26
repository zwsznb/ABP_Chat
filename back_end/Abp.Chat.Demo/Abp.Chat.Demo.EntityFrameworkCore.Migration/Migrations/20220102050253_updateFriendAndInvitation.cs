using Microsoft.EntityFrameworkCore.Migrations;

namespace Abp.Chat.Demo.EntityFrameworkCore.Migrations.Migrations
{
    public partial class updateFriendAndInvitation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReaded",
                table: "Invitation",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Nickname",
                table: "Friend",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReaded",
                table: "Invitation");

            migrationBuilder.DropColumn(
                name: "Nickname",
                table: "Friend");
        }
    }
}
