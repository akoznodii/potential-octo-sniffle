using Microsoft.EntityFrameworkCore.Migrations;

namespace Checkout.PaymentGateway.Infrastructure.Storage.Migrations
{
    public partial class AddClientsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Clients",
                schema: "gateway",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    Key = table.Column<string>(nullable: true),
                    Role = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Name);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Clients",
                schema: "gateway");
        }
    }
}
