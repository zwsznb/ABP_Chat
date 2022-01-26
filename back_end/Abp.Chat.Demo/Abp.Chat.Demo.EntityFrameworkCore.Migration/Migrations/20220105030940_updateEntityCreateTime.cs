using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Abp.Chat.Demo.EntityFrameworkCore.Migrations.Migrations
{
    public partial class updateEntityCreateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "ChatInformation",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Created",
                table: "ChatInformation");
        }
    }
}
