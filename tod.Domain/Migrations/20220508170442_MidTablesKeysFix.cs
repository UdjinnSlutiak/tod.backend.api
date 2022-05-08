using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tod.Domain.Migrations
{
    public partial class MidTablesKeysFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserTopics_UserId",
                table: "UserTopics");

            migrationBuilder.DropIndex(
                name: "IX_UserTags_UserId",
                table: "UserTags");

            migrationBuilder.DropIndex(
                name: "IX_UserReports_UserId",
                table: "UserReports");

            migrationBuilder.DropIndex(
                name: "IX_UserCommentaries_UserId",
                table: "UserCommentaries");

            migrationBuilder.DropIndex(
                name: "IX_TopicTags_TopicId",
                table: "TopicTags");

            migrationBuilder.DropIndex(
                name: "IX_TopicReports_TopicId",
                table: "TopicReports");

            migrationBuilder.DropIndex(
                name: "IX_TopicReactions_TopicId",
                table: "TopicReactions");

            migrationBuilder.DropIndex(
                name: "IX_TopicCommentaries_TopicId",
                table: "TopicCommentaries");

            migrationBuilder.DropIndex(
                name: "IX_FavoriteTopics_UserId",
                table: "FavoriteTopics");

            migrationBuilder.DropIndex(
                name: "IX_CommentaryReports_CommentaryId",
                table: "CommentaryReports");

            migrationBuilder.DropIndex(
                name: "IX_CommentaryReactions_CommentaryId",
                table: "CommentaryReactions");

            migrationBuilder.DropColumn(
                name: "ReactionType",
                table: "Reactions");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Topics",
                newName: "CreatedUtc");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Reactions",
                newName: "CreatedUtc");

            migrationBuilder.RenameColumn(
                name: "Created",
                table: "Commentaries",
                newName: "CreatedUtc");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTopics",
                table: "UserTopics",
                columns: new[] { "UserId", "TopicId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserTags",
                table: "UserTags",
                columns: new[] { "UserId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserReports",
                table: "UserReports",
                columns: new[] { "UserId", "ReportId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserCommentaries",
                table: "UserCommentaries",
                columns: new[] { "UserId", "CommentaryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopicTags",
                table: "TopicTags",
                columns: new[] { "TopicId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopicReports",
                table: "TopicReports",
                columns: new[] { "TopicId", "ReportId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopicReactions",
                table: "TopicReactions",
                columns: new[] { "TopicId", "ReactionId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_TopicCommentaries",
                table: "TopicCommentaries",
                columns: new[] { "TopicId", "CommentaryId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_FavoriteTopics",
                table: "FavoriteTopics",
                columns: new[] { "UserId", "TopicId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentaryReports",
                table: "CommentaryReports",
                columns: new[] { "CommentaryId", "ReportId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CommentaryReactions",
                table: "CommentaryReactions",
                columns: new[] { "CommentaryId", "ReactionId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTopics",
                table: "UserTopics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserTags",
                table: "UserTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserReports",
                table: "UserReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserCommentaries",
                table: "UserCommentaries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopicTags",
                table: "TopicTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopicReports",
                table: "TopicReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopicReactions",
                table: "TopicReactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TopicCommentaries",
                table: "TopicCommentaries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FavoriteTopics",
                table: "FavoriteTopics");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentaryReports",
                table: "CommentaryReports");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CommentaryReactions",
                table: "CommentaryReactions");

            migrationBuilder.RenameColumn(
                name: "CreatedUtc",
                table: "Topics",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "CreatedUtc",
                table: "Reactions",
                newName: "Created");

            migrationBuilder.RenameColumn(
                name: "CreatedUtc",
                table: "Commentaries",
                newName: "Created");

            migrationBuilder.AddColumn<int>(
                name: "ReactionType",
                table: "Reactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserTopics_UserId",
                table: "UserTopics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTags_UserId",
                table: "UserTags",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserReports_UserId",
                table: "UserReports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentaries_UserId",
                table: "UserCommentaries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicTags_TopicId",
                table: "TopicTags",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicReports_TopicId",
                table: "TopicReports",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicReactions_TopicId",
                table: "TopicReactions",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_TopicCommentaries_TopicId",
                table: "TopicCommentaries",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteTopics_UserId",
                table: "FavoriteTopics",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentaryReports_CommentaryId",
                table: "CommentaryReports",
                column: "CommentaryId");

            migrationBuilder.CreateIndex(
                name: "IX_CommentaryReactions_CommentaryId",
                table: "CommentaryReactions",
                column: "CommentaryId");
        }
    }
}
