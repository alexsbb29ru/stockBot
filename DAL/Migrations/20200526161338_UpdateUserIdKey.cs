using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class UpdateUserIdKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Statistics_StatisticStatId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_StatisticStatId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "StatisticStatId",
                table: "Users");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Statistics",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Statistics");

            migrationBuilder.AddColumn<Guid>(
                name: "StatisticStatId",
                table: "Users",
                type: "uniqueidentifier",
                nullable: true);

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
    }
}
