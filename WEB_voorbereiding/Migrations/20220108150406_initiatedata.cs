using Microsoft.EntityFrameworkCore.Migrations;

namespace WebHelper.Migrations
{
    public partial class initiatedata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Functie",
                table: "Gebruikers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Functie",
                table: "Gebruikers");
        }
    }
}
