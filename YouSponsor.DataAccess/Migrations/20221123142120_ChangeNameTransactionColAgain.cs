using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SponsorY.DataAccess.Migrations
{
    public partial class ChangeNameTransactionColAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllUserSponsorId",
                table: "Transactions",
                newName: "UserSponsorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserSponsorId",
                table: "Transactions",
                newName: "AllUserSponsorId");
        }
    }
}
