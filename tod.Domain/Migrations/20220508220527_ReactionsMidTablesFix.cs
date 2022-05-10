using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tod.Domain.Migrations
{
    public partial class ReactionsMidTablesFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentaryReactions");

            migrationBuilder.DropTable(
                name: "TopicReactions");

            migrationBuilder.CreateTable(
                name: "UserCommentaryReactions",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    CommentaryId = table.Column<int>(type: "int", nullable: false),
                    ReactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCommentaryReactions", x => new { x.UserId, x.CommentaryId, x.ReactionId });
                    table.ForeignKey(
                        name: "FK_UserCommentaryReactions_Commentaries_CommentaryId",
                        column: x => x.CommentaryId,
                        principalTable: "Commentaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTopicReactions",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    ReactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTopicReactions", x => new { x.UserId, x.TopicId, x.ReactionId });
                    table.ForeignKey(
                        name: "FK_UserTopicReactions_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserCommentaryReactions_CommentaryId",
                table: "UserCommentaryReactions",
                column: "CommentaryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTopicReactions_TopicId",
                table: "UserTopicReactions",
                column: "TopicId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserCommentaryReactions");

            migrationBuilder.DropTable(
                name: "UserTopicReactions");

            migrationBuilder.CreateTable(
                name: "CommentaryReactions",
                columns: table => new
                {
                    CommentaryId = table.Column<int>(type: "int", nullable: false),
                    ReactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentaryReactions", x => new { x.CommentaryId, x.ReactionId });
                    table.ForeignKey(
                        name: "FK_CommentaryReactions_Commentaries_CommentaryId",
                        column: x => x.CommentaryId,
                        principalTable: "Commentaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TopicReactions",
                columns: table => new
                {
                    TopicId = table.Column<int>(type: "int", nullable: false),
                    ReactionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TopicReactions", x => new { x.TopicId, x.ReactionId });
                    table.ForeignKey(
                        name: "FK_TopicReactions_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
