using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SponsorY.DataAccess.Migrations
{
    public partial class test : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Sponsorships",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Sponsorships_SponsorshipId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_SponsorshipId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Sponsorships");

            migrationBuilder.DropColumn(
                name: "SponsorshipId",
                table: "Categories");
        }
    }
}
