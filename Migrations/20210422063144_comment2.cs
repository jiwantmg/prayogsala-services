using Microsoft.EntityFrameworkCore.Migrations;

namespace prayogsala_services.Migrations
{
    public partial class comment2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_comments_CourseId",
                table: "comments",
                column: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_comments_courses_CourseId",
                table: "comments",
                column: "CourseId",
                principalTable: "courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comments_courses_CourseId",
                table: "comments");

            migrationBuilder.DropIndex(
                name: "IX_comments_CourseId",
                table: "comments");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "comments");
        }
    }
}
