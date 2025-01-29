using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace pbadraH60A01.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedCustomerTableColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "CartItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CartItems",
                keyColumn: "CartItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ShoppingCarts",
                keyColumn: "CartId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValue: 2);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Customers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreditCard",
                table: "Customers",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "CreditCard",
                table: "Customers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(16)",
                oldMaxLength: 16,
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "CreditCard", "Email", "FirstName", "LastName", "Password", "PhoneNumber", "Province", "UserId" },
                values: new object[,]
                {
                    { 1, "4111 1111 1111 1111", "rchan@gmail.com", "Richard", "Chan", null, "8192346544", "ON", null },
                    { 2, "5500 0000 0000 0004", "emily.johnson@example.com", "Emily", "Johnson", null, "6134567890", "QC", null },
                    { 3, "3400 0000 0000 009", "michael.smith@example.com", "Michael", "Smith", null, "5147891234", "BC", null }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CustomerId", "DateCreated", "DateFulfilled", "Taxes", "Total" },
                values: new object[] { 1, 1, new DateTime(2024, 10, 18, 3, 48, 26, 281, DateTimeKind.Utc).AddTicks(5361), new DateTime(2024, 10, 18, 3, 48, 26, 281, DateTimeKind.Utc).AddTicks(5362), 13999.99m, 139999.97m });

            migrationBuilder.InsertData(
                table: "ShoppingCarts",
                columns: new[] { "CartId", "CustomerId", "DateCreated" },
                values: new object[] { 1, 2, new DateTime(2024, 10, 18, 3, 48, 26, 281, DateTimeKind.Utc).AddTicks(5311) });

            migrationBuilder.InsertData(
                table: "CartItems",
                columns: new[] { "CartItemId", "CartId", "Price", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 22000.00m, 9, 3 },
                    { 2, 1, 12000.00m, 17, 1 }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderItemId", "OrderId", "Price", "ProductId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 75000.00m, 1, 1 },
                    { 2, 1, 35000.00m, 5, 1 },
                    { 3, 1, 26000.00m, 9, 1 }
                });
        }
    }
}
