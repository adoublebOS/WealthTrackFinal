using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace WealthTrack.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletLimit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_SavingsPlans_SavingsId",
                table: "Transactions");

            migrationBuilder.RenameColumn(
                name: "SavingsId",
                table: "Transactions",
                newName: "SavingsPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SavingsId",
                table: "Transactions",
                newName: "IX_Transactions_SavingsPlanId");

            migrationBuilder.CreateTable(
                name: "WalletLimits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    WalletId = table.Column<int>(type: "integer", nullable: true),
                    LimitAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    Month = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletLimits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletLimits_Wallets_WalletId",
                        column: x => x.WalletId,
                        principalTable: "Wallets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WalletLimits_WalletId",
                table: "WalletLimits",
                column: "WalletId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_SavingsPlans_SavingsPlanId",
                table: "Transactions",
                column: "SavingsPlanId",
                principalTable: "SavingsPlans",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_SavingsPlans_SavingsPlanId",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "WalletLimits");

            migrationBuilder.RenameColumn(
                name: "SavingsPlanId",
                table: "Transactions",
                newName: "SavingsId");

            migrationBuilder.RenameIndex(
                name: "IX_Transactions_SavingsPlanId",
                table: "Transactions",
                newName: "IX_Transactions_SavingsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_SavingsPlans_SavingsId",
                table: "Transactions",
                column: "SavingsId",
                principalTable: "SavingsPlans",
                principalColumn: "Id");
        }
    }
}
