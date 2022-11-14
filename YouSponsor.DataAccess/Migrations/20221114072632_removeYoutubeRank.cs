using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SponsorY.DataAccess.Migrations
{
    public partial class removeYoutubeRank : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Youtubers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Rank",
                table: "Youtubers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
