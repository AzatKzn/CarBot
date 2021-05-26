using Microsoft.EntityFrameworkCore.Migrations;

namespace CarBot.Migrations
{
    public partial class _2021052300 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Сunning",
                table: "Users",
                newName: "Cunning");

            migrationBuilder.AddColumn<string>(
                name: "Hash",
                table: "GroupRaces",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "RaceDivision",
                table: "GroupRaces",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Experience",
                table: "GroupRaceParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Money",
                table: "GroupRaceParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Place",
                table: "GroupRaceParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "GroupRaceParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Hash",
                table: "GroupRaces");

            migrationBuilder.DropColumn(
                name: "RaceDivision",
                table: "GroupRaces");

            migrationBuilder.DropColumn(
                name: "Experience",
                table: "GroupRaceParticipant");

            migrationBuilder.DropColumn(
                name: "Money",
                table: "GroupRaceParticipant");

            migrationBuilder.DropColumn(
                name: "Place",
                table: "GroupRaceParticipant");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "GroupRaceParticipant");

            migrationBuilder.RenameColumn(
                name: "Cunning",
                table: "Users",
                newName: "Сunning");
        }
    }
}
