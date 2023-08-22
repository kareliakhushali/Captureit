using Microsoft.EntityFrameworkCore.Migrations;

namespace Captureit.Migrations
{
    public partial class done2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attendance",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Attendance",
                table: "Users",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
