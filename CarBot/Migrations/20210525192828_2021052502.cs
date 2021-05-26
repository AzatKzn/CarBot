using Microsoft.EntityFrameworkCore.Migrations;

namespace CarBot.Migrations
{
    public partial class _2021052502 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserCarId",
                table: "GroupRaceParticipant",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupRaceParticipant_UserCarId",
                table: "GroupRaceParticipant",
                column: "UserCarId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupRaceParticipant_Cars_UserCarId",
                table: "GroupRaceParticipant",
                column: "UserCarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupRaceParticipant_Cars_UserCarId",
                table: "GroupRaceParticipant");

            migrationBuilder.DropIndex(
                name: "IX_GroupRaceParticipant_UserCarId",
                table: "GroupRaceParticipant");

            migrationBuilder.DropColumn(
                name: "UserCarId",
                table: "GroupRaceParticipant");
        }
    }
}
