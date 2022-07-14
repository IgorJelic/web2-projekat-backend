using Microsoft.EntityFrameworkCore.Migrations;

namespace Backend.Migrations
{
    public partial class PorudzbinaIsporucena : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Isporucena",
                table: "Porudzbine",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Isporucena",
                table: "Porudzbine");
        }
    }
}
