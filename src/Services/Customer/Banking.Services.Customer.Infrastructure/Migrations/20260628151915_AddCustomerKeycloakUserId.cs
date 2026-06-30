using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banking.Services.Customer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomerKeycloakUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KeycloakUserId",
                table: "Customers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_KeycloakUserId",
                table: "Customers",
                column: "KeycloakUserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Customers_KeycloakUserId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "KeycloakUserId",
                table: "Customers");
        }
    }
}
