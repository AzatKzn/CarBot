using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarBot.Migrations
{
    public partial class _2021051000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Braking",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Mobility",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Overclocking",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Cars");

            migrationBuilder.AddColumn<int>(
                name: "AutoId",
                table: "Cars",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Autos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IsEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Property = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PropertyValue = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Speed = table.Column<int>(type: "int", nullable: false),
                    Mobility = table.Column<int>(type: "int", nullable: false),
                    Overclocking = table.Column<int>(type: "int", nullable: false),
                    Braking = table.Column<int>(type: "int", nullable: false),
                    Cost = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Autos", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_AutoId",
                table: "Cars",
                column: "AutoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Autos_AutoId",
                table: "Cars",
                column: "AutoId",
                principalTable: "Autos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Autos_AutoId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "Autos");

            migrationBuilder.DropIndex(
                name: "IX_Cars_AutoId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "AutoId",
                table: "Cars");

            migrationBuilder.AddColumn<int>(
                name: "Braking",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Mobility",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Cars",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "Overclocking",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Speed",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
