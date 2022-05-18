using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tod.Domain.Migrations
{
    public partial class UpdatedForeignKeys : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_UserTopics_TopicId",
                table: "UserTopics",
                column: "TopicId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserTopicReactions_ReactionId",
                table: "UserTopicReactions",
                column: "ReactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserReports_ReportId",
                table: "UserReports",
                column: "ReportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentaryReactions_ReactionId",
                table: "UserCommentaryReactions",
                column: "ReactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentaries_CommentaryId",
                table: "UserCommentaries",
                column: "CommentaryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopicReports_ReportId",
                table: "TopicReports",
                column: "ReportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TopicCommentaries_CommentaryId",
                table: "TopicCommentaries",
                column: "CommentaryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CommentaryReports_ReportId",
                table: "CommentaryReports",
                column: "ReportId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CommentaryReports_Reports_ReportId",
                table: "CommentaryReports",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicCommentaries_Commentaries_CommentaryId",
                table: "TopicCommentaries",
                column: "CommentaryId",
                principalTable: "Commentaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TopicReports_Reports_ReportId",
                table: "TopicReports",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCommentaries_Commentaries_CommentaryId",
                table: "UserCommentaries",
                column: "CommentaryId",
                principalTable: "Commentaries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCommentaryReactions_Reactions_ReactionId",
                table: "UserCommentaryReactions",
                column: "ReactionId",
                principalTable: "Reactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserCommentaryReactions_Users_UserId",
                table: "UserCommentaryReactions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserReports_Reports_ReportId",
                table: "UserReports",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTopicReactions_Reactions_ReactionId",
                table: "UserTopicReactions",
                column: "ReactionId",
                principalTable: "Reactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTopicReactions_Users_UserId",
                table: "UserTopicReactions",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserTopics_Topics_TopicId",
                table: "UserTopics",
                column: "TopicId",
                principalTable: "Topics",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CommentaryReports_Reports_ReportId",
                table: "CommentaryReports");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicCommentaries_Commentaries_CommentaryId",
                table: "TopicCommentaries");

            migrationBuilder.DropForeignKey(
                name: "FK_TopicReports_Reports_ReportId",
                table: "TopicReports");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCommentaries_Commentaries_CommentaryId",
                table: "UserCommentaries");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCommentaryReactions_Reactions_ReactionId",
                table: "UserCommentaryReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserCommentaryReactions_Users_UserId",
                table: "UserCommentaryReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserReports_Reports_ReportId",
                table: "UserReports");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTopicReactions_Reactions_ReactionId",
                table: "UserTopicReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTopicReactions_Users_UserId",
                table: "UserTopicReactions");

            migrationBuilder.DropForeignKey(
                name: "FK_UserTopics_Topics_TopicId",
                table: "UserTopics");

            migrationBuilder.DropIndex(
                name: "IX_UserTopics_TopicId",
                table: "UserTopics");

            migrationBuilder.DropIndex(
                name: "IX_UserTopicReactions_ReactionId",
                table: "UserTopicReactions");

            migrationBuilder.DropIndex(
                name: "IX_UserReports_ReportId",
                table: "UserReports");

            migrationBuilder.DropIndex(
                name: "IX_UserCommentaryReactions_ReactionId",
                table: "UserCommentaryReactions");

            migrationBuilder.DropIndex(
                name: "IX_UserCommentaries_CommentaryId",
                table: "UserCommentaries");

            migrationBuilder.DropIndex(
                name: "IX_TopicReports_ReportId",
                table: "TopicReports");

            migrationBuilder.DropIndex(
                name: "IX_TopicCommentaries_CommentaryId",
                table: "TopicCommentaries");

            migrationBuilder.DropIndex(
                name: "IX_CommentaryReports_ReportId",
                table: "CommentaryReports");
        }
    }
}
