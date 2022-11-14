using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SponsorY.DataAccess.Migrations
{
    public partial class YoutuberCategoryNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Sponsorships_SponsorshipId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Youtubers_Categories_CategoryId",
                table: "Youtubers");

            migrationBuilder.DropIndex(
                name: "IX_Categories_SponsorshipId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "SponsorshipId",
                table: "Categories");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Youtubers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Youtubers_Categories_CategoryId",
                table: "Youtubers",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Youtubers_Categories_CategoryId",
                table: "Youtubers");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Youtubers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SponsorshipId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_SponsorshipId",
                table: "Categories",
                column: "SponsorshipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Sponsorships_SponsorshipId",
                table: "Categories",
                column: "SponsorshipId",
                principalTable: "Sponsorships",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Youtubers_Categories_CategoryId",
                table: "Youtubers",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
