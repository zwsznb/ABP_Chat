using Microsoft.EntityFrameworkCore.Migrations;

namespace Abp.Chat.Demo.EntityFrameworkCore.Migrations.Migrations
{
    public partial class updateInvitationIsAccept : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSendFinish",
                table: "Invitation");

            migrationBuilder.AddColumn<int>(
                name: "IsAccept",
                table: "Invitation",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccept",
                table: "Invitation");

            migrationBuilder.AddColumn<bool>(
                name: "IsSendFinish",
                table: "Invitation",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
