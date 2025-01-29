using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace pbadraH60A01.Migrations
{
    /// <inheritdoc />
    public partial class initialDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Province = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditCard = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdCat = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateFulfilled = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Total = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Taxes = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    CartId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustomerId = table.Column<int>(type: "int", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.CartId);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProdCatId = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Manufacturer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Stock = table.Column<int>(type: "int", nullable: true),
                    BuyPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SellPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_ProdCatId",
                        column: x => x.ProdCatId,
                        principalTable: "ProductCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    CartItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.CartItemId);
                    table.ForeignKey(
                        name: "FK_CartItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_ShoppingCarts_CartId",
                        column: x => x.CartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "CartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ProductId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.OrderItemId);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "CreditCard", "Email", "FirstName", "LastName", "PhoneNumber", "Province" },
                values: new object[,]
                {
                    { 1, "4111 1111 1111 1111", "rchan@gmail.com", "Richard", "Chan", "8192346544", "ON" },
                    { 2, "5500 0000 0000 0004", "emily.johnson@example.com", "Emily", "Johnson", "6134567890", "QC" },
                    { 3, "3400 0000 0000 009", "michael.smith@example.com", "Michael", "Smith", "5147891234", "BC" }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "CategoryId", "ProdCat" },
                values: new object[,]
                {
                    { 1, "Sailboats" },
                    { 2, "Motorboats" },
                    { 3, "Fishing Boats" },
                    { 4, "Yachts" },
                    { 5, "Jet Skis" }
                });

            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "CustomerId", "DateCreated", "DateFulfilled", "Taxes", "Total" },
                values: new object[] { 1, 1, new DateTime(2024, 9, 9, 7, 2, 23, 410, DateTimeKind.Utc).AddTicks(2384), new DateTime(2024, 9, 9, 7, 2, 23, 410, DateTimeKind.Utc).AddTicks(2385), 13999.99m, 139999.97m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "ProductId", "BuyPrice", "Description", "Manufacturer", "ProdCatId", "SellPrice", "Stock" },
                values: new object[,]
                {
                    { 1, 65000.00m, "Beneteau Oceanis 30.1", "Beneteau", 1, 75000.00m, 5 },
                    { 2, 45000.00m, "Catalina 320", "Catalina Yachts", 1, 55000.00m, 3 },
                    { 3, 120000.00m, "Jeanneau Sun Odyssey 410", "Jeanneau", 1, 140000.00m, 4 },
                    { 4, 35000.00m, "Hunter 33", "Hunter Marine", 1, 45000.00m, 2 },
                    { 5, 30000.00m, "Sea Ray SPX 190", "Sea Ray", 2, 35000.00m, 7 },
                    { 6, 55000.00m, "Chaparral 23 SSi", "Chaparral Boats", 2, 60000.00m, 5 },
                    { 7, 25000.00m, "Bayliner VR5", "Bayliner", 2, 30000.00m, 8 },
                    { 8, 75000.00m, "Cobalt R30", "Cobalt Boats", 2, 85000.00m, 3 },
                    { 9, 22000.00m, "Lund 1775 Crossover XS", "Lund Boats", 3, 26000.00m, 10 },
                    { 10, 26000.00m, "Tracker Pro Guide V-175 Combo", "Tracker Boats", 3, 30000.00m, 6 },
                    { 11, 35000.00m, "Ranger Z518", "Ranger Boats", 3, 40000.00m, 4 },
                    { 12, 45000.00m, "Bass Cat Cougar FTD", "Bass Cat Boats", 3, 50000.00m, 2 },
                    { 13, 800000.00m, "Sunseeker Predator 55", "Sunseeker", 4, 950000.00m, 1 },
                    { 14, 950000.00m, "Princess V60", "Princess Yachts", 4, 1150000.00m, 1 },
                    { 15, 600000.00m, "Fairline Targa 45", "Fairline", 4, 700000.00m, 2 },
                    { 16, 1400000.00m, "Azimut S6", "Azimut", 4, 1600000.00m, 1 },
                    { 17, 12000.00m, "Sea-Doo GTX 170", "Sea-Doo", 5, 15000.00m, 10 },
                    { 18, 8000.00m, "Yamaha EX Deluxe", "Yamaha", 5, 10000.00m, 8 },
                    { 19, 15000.00m, "Kawasaki Jet Ski Ultra 310R", "Kawasaki", 5, 18000.00m, 5 },
                    { 20, 14000.00m, "Sea-Doo RXT-X 300", "Sea-Doo", 5, 17000.00m, 7 }
                });

            migrationBuilder.InsertData(
                table: "ShoppingCarts",
                columns: new[] { "CartId", "CustomerId", "DateCreated" },
                values: new object[] { 1, 2, new DateTime(2024, 9, 9, 7, 2, 23, 410, DateTimeKind.Utc).AddTicks(2336) });

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

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ProductId",
                table: "CartItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProdCatId",
                table: "Products",
                column: "ProdCatId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_CustomerId",
                table: "ShoppingCarts",
                column: "CustomerId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "ProductCategories");
        }
    }
}
