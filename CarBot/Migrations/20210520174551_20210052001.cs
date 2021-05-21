using Microsoft.EntityFrameworkCore.Migrations;

namespace CarBot.Migrations
{
    public partial class _20210052001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FactUp",
                table: "RewardHistories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsExp",
                table: "RewardHistories",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FactUp",
                table: "RewardHistories");

            migrationBuilder.DropColumn(
                name: "IsExp",
                table: "RewardHistories");
        }
    }
}
