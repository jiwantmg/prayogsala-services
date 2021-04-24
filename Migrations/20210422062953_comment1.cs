using Microsoft.EntityFrameworkCore.Migrations;

namespace prayogsala_services.Migrations
{
    public partial class comment1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChapterId",
                table: "comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TopicId",
                table: "comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_comments_ChapterId",
                table: "comments",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_comments_TopicId",
                table: "comments",
                column: "TopicId");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_chapters_ChapterId",
                table: "comments",
                column: "ChapterId",
                principalTable: "chapters",
                principalColumn: "ChapterId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_comments_topics_TopicId",
                table: "comments",
                column: "TopicId",
                principalTable: "topics",
                principalColumn: "TopicId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_chapters_ChapterId",
                table: "comments");

            migrationBuilder.DropForeignKey(
                name: "FK_comments_topics_TopicId",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "IX_comments_ChapterId",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "IX_comments_TopicId",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "ChapterId",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "TopicId",
                table: "comments");
        }
    }
}
