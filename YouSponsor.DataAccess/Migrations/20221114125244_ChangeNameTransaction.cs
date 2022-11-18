using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SponsorY.DataAccess.Migrations
{
    public partial class ChangeNameTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Youtubers_Transfers_TransferId",
                table: "Youtubers");

            migrationBuilder.DropTable(
                name: "Transfers");

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransferMoveney = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NumberOfSponsorship = table.Column<int>(type: "int", nullable: false),
                    SponsorshipId = table.Column<int>(type: "int", nullable: false),
                    YoutuberId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Sponsorships_SponsorshipId",
                        column: x => x.SponsorshipId,
                        principalTable: "Sponsorships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SponsorshipId",
                table: "Transactions",
                column: "SponsorshipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Youtubers_Transactions_TransferId",
                table: "Youtubers",
                column: "TransferId",
                principalTable: "Transactions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Youtubers_Transactions_TransferId",
                table: "Youtubers");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.CreateTable(
                name: "Transfers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SponsorshipId = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfSponsorship = table.Column<int>(type: "int", nullable: false),
                    TransferMoveney = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    YoutuberId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transfers_Sponsorships_SponsorshipId",
                        column: x => x.SponsorshipId,
                        principalTable: "Sponsorships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transfers_SponsorshipId",
                table: "Transfers",
                column: "SponsorshipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Youtubers_Transfers_TransferId",
                table: "Youtubers",
                column: "TransferId",
                principalTable: "Transfers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
