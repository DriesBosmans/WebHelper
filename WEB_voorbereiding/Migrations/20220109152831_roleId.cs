using Microsoft.EntityFrameworkCore.Migrations;

namespace WEB_voorbereiding.Migrations
{
    public partial class roleId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RoleId",
                table: "Gebruikers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Gebruikers");
        }
    }
}
