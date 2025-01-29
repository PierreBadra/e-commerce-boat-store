using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pbadraH60A01.Migrations
{
    /// <inheritdoc />
    public partial class IdentitySetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateFulfilled" },
                values: new object[] { new DateTime(2024, 10, 2, 22, 1, 19, 857, DateTimeKind.Utc).AddTicks(712), new DateTime(2024, 10, 2, 22, 1, 19, 857, DateTimeKind.Utc).AddTicks(713) });

            migrationBuilder.UpdateData(
                table: "ShoppingCarts",
                keyColumn: "CartId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2024, 10, 2, 22, 1, 19, 857, DateTimeKind.Utc).AddTicks(667));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1,
                columns: new[] { "DateCreated", "DateFulfilled" },
                values: new object[] { new DateTime(2024, 9, 9, 7, 2, 23, 410, DateTimeKind.Utc).AddTicks(2384), new DateTime(2024, 9, 9, 7, 2, 23, 410, DateTimeKind.Utc).AddTicks(2385) });

            migrationBuilder.UpdateData(
                table: "ShoppingCarts",
                keyColumn: "CartId",
                keyValue: 1,
                column: "DateCreated",
                value: new DateTime(2024, 9, 9, 7, 2, 23, 410, DateTimeKind.Utc).AddTicks(2336));
        }
    }
}
