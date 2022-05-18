using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tod.Domain.Migrations
{
    public partial class RemoveUserIdFromSomeEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Reactions");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Reactions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
