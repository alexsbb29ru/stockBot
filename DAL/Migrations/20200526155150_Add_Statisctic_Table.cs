using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Add_Statisctic_Table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StatisticStatId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    StatId = table.Column<Guid>(nullable: false),
                    StatDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => x.StatId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_StatisticStatId",
                table: "Users",
                column: "StatisticStatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Statistics_StatisticStatId",
                table: "Users",
                column: "StatisticStatId",
                principalTable: "Statistics",
                principalColumn: "StatId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
