using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace prayogsala_services.Migrations
{
    public partial class video : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VideoId",
                table: "topics",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "videos",
                columns: table => new
                {
                    VideoId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    VideoName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_videos", x => x.VideoId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_topics_VideoId",
                table: "topics",
                column: "VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_topics_videos_VideoId",
                table: "topics",
                column: "VideoId",
                principalTable: "videos",
                principalColumn: "VideoId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_topics_videos_VideoId",
                table: "topics");

            migrationBuilder.DropTable(
                name: "videos");

            migrationBuilder.DropIndex(
                name: "IX_topics_VideoId",
                table: "topics");

            migrationBuilder.DropColumn(
                name: "VideoId",
                table: "topics");
        }
    }
}
