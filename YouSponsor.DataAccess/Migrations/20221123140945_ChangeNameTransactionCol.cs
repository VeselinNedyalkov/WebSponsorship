using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SponsorY.DataAccess.Migrations
{
    public partial class ChangeNameTransactionCol : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllUserSponsor",
                table: "Transactions",
                newName: "AllUserSponsorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AllUserSponsorId",
                table: "Transactions",
                newName: "AllUserSponsor");
        }
    }
}
