using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tod.Domain.Migrations
{
    public partial class ReactionTablesRedesign : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserReports_Reports_ReportId",
                table: "UserReports");

            migrationBuilder.DropIndex(
                name: "IX_UserReports_ReportId",
                table: "UserReports");

            migrationBuilder.CreateIndex(
                name: "IX_TopicReports_TopicId",
                table: "TopicReports",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentaryReports_CommentaryId",
                table: "CommentaryReports",
                column: "CommentaryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CommentaryReports_Commentaries_CommentaryId",
                table: "CommentaryReports",
                column: "CommentaryId",
                principalTable: "Commentaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicReports_Topics_TopicId",
                table: "TopicReports",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentaryReports_Commentaries_CommentaryId",
                table: "CommentaryReports");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicReports_Topics_TopicId",
                table: "TopicReports");

            migrationBuilder.DropIndex(
                name: "IX_TopicReports_TopicId",
                table: "TopicReports");

            migrationBuilder.DropIndex(
                name: "IX_CommentaryReports_CommentaryId",
                table: "CommentaryReports");

            migrationBuilder.CreateIndex(
                name: "IX_UserReports_ReportId",
                table: "UserReports",
                column: "ReportId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserReports_Reports_ReportId",
                table: "UserReports",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
