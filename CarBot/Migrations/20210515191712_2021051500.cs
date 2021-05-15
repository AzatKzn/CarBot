using Microsoft.EntityFrameworkCore.Migrations;

namespace CarBot.Migrations
{
    public partial class _2021051500 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TestDrivesCount",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestDrivesCount",
                table: "Users");
        }
    }
}
