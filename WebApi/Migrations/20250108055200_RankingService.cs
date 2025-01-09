using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class RankingService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "GameResults");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "GameResults",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ClearScore",
                table: "GameResults",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "KillScore",
                table: "GameResults",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "PlayTime",
                table: "GameResults",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "UserPassword",
                table: "GameResults",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClearScore",
                table: "GameResults");

            migrationBuilder.DropColumn(
                name: "KillScore",
                table: "GameResults");

            migrationBuilder.DropColumn(
                name: "PlayTime",
                table: "GameResults");

            migrationBuilder.DropColumn(
                name: "UserPassword",
                table: "GameResults");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "GameResults",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "GameResults",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
