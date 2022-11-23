using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SponsorY.DataAccess.Migrations
{
    public partial class Test1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Transactions",
                newName: "AllUserSponsor");

            migrationBuilder.RenameColumn(
                name: "NumberOfSponsorship",
                table: "Transactions",
                newName: "QuntityClips");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuntityClips",
                table: "Transactions",
                newName: "NumberOfSponsorship");

            migrationBuilder.RenameColumn(
                name: "AllUserSponsor",
                table: "Transactions",
                newName: "UserId");
        }
    }
}
