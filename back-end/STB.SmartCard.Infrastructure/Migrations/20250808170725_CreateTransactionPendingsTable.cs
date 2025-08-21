using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace STB.SmartCard.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTransactionPendingsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TransactionPendings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Montant = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Lieu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NomBanque = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeRetrait = table.Column<int>(type: "int", nullable: true),
                    CompteDestinataire = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SourcePaiement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CarteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OtpCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsValidated = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionPendings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionPendings_Cartes_CarteId",
                        column: x => x.CarteId,
                        principalTable: "Cartes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TransactionPendings_CarteId",
                table: "TransactionPendings",
                column: "CarteId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TransactionPendings");
        }
    }
}
