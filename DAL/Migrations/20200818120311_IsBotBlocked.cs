using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class IsBotBlocked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBotBlocked",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBotBlocked",
                table: "Users");
        }
    }
}
