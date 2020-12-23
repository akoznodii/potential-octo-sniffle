using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Checkout.PaymentGateway.Infrastructure.Storage.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "gateway");

            migrationBuilder.CreateTable(
                name: "Merchants",
                schema: "gateway",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DisplayName = table.Column<string>(nullable: true),
                    CallbackApiUrl = table.Column<string>(nullable: true),
                    CallbackApiUsername = table.Column<string>(nullable: true),
                    CallbackApiPassword = table.Column<string>(nullable: true),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Merchants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                schema: "gateway",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MerchantId = table.Column<Guid>(nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(nullable: true),
                    ProcessorTransactionId = table.Column<string>(nullable: true),
                    ProcessorTransactionStatus = table.Column<string>(nullable: true),
                    CreditCardType = table.Column<string>(nullable: true),
                    CreditCardNumber = table.Column<string>(nullable: true),
                    CreditCardExpiryMonth = table.Column<int>(nullable: false),
                    CreditCardExpiryYear = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    Timestamp = table.Column<byte[]>(rowVersion: true, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalSchema: "gateway",
                        principalTable: "Merchants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_MerchantId",
                schema: "gateway",
                table: "Payments",
                column: "MerchantId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Payments",
                schema: "gateway");

            migrationBuilder.DropTable(
                name: "Merchants",
                schema: "gateway");
        }
    }
}
