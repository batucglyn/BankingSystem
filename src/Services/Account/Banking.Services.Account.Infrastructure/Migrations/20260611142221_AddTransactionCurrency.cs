using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Banking.Services.Account.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionCurrency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Currency",
                table: "AccountTransactions",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "AccountTransactions");
        }
    }
}
