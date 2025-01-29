using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pbadraH60A01.Migrations
{
    /// <inheritdoc />
    public partial class AddPasswordColumnCustomerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Password",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1,
                column: "Password",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2,
                column: "Password",
                value: null);

            migrationBuilder.UpdateData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 3,
                column: "Password",
                value: null);

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateFulfilled" },
                values: new object[] { new DateTime(2024, 10, 18, 3, 48, 26, 281, DateTimeKind.Utc).AddTicks(5361), new DateTime(2024, 10, 18, 3, 48, 26, 281, DateTimeKind.Utc).AddTicks(5362) });

            migrationBuilder.UpdateData(
                table: "ShoppingCarts",
                keyColumn: "CartId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2024, 10, 18, 3, 48, 26, 281, DateTimeKind.Utc).AddTicks(5311));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Password",
                table: "Customers");

            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateFulfilled" },
                values: new object[] { new DateTime(2024, 10, 15, 23, 50, 58, 734, DateTimeKind.Utc).AddTicks(7966), new DateTime(2024, 10, 15, 23, 50, 58, 734, DateTimeKind.Utc).AddTicks(7967) });

            migrationBuilder.UpdateData(
                table: "ShoppingCarts",
                keyColumn: "CartId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2024, 10, 15, 23, 50, 58, 734, DateTimeKind.Utc).AddTicks(7917));
        }
    }
}
