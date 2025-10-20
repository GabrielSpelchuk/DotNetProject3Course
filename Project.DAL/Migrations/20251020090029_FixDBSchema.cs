using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Project.DAL.Migrations
{
    /// <inheritdoc />
    public partial class FixDBSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "catalog_db");

            migrationBuilder.EnsureSchema(
                name: "orders_db");

            migrationBuilder.CreateTable(
                name: "categories",
                schema: "catalog_db",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                schema: "orders_db",
                columns: table => new
                {
                    CustomerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_customers", x => x.CustomerId);
                });

            migrationBuilder.CreateTable(
                name: "suppliers",
                schema: "catalog_db",
                columns: table => new
                {
                    SupplierId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Country = table.Column<string>(type: "text", nullable: false),
                    Rating = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_suppliers", x => x.SupplierId);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                schema: "orders_db",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    SupplierId = table.Column<int>(type: "integer", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_orders_customers_CustomerId",
                        column: x => x.CustomerId,
                        principalSchema: "orders_db",
                        principalTable: "customers",
                        principalColumn: "CustomerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_orders_suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "catalog_db",
                        principalTable: "suppliers",
                        principalColumn: "SupplierId");
                });

            migrationBuilder.CreateTable(
                name: "products",
                schema: "catalog_db",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SupplierId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_products_suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalSchema: "catalog_db",
                        principalTable: "suppliers",
                        principalColumn: "SupplierId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                schema: "orders_db",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.PaymentId);
                    table.ForeignKey(
                        name: "FK_payments_orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "orders_db",
                        principalTable: "orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order_items",
                schema: "orders_db",
                columns: table => new
                {
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_order_items", x => new { x.OrderId, x.ProductId });
                    table.ForeignKey(
                        name: "FK_order_items_orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "orders_db",
                        principalTable: "orders",
                        principalColumn: "OrderId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_order_items_products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "catalog_db",
                        principalTable: "products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_categories",
                schema: "catalog_db",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_categories", x => new { x.ProductId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_product_categories_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "catalog_db",
                        principalTable: "categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_product_categories_products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "catalog_db",
                        principalTable: "products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_details",
                schema: "catalog_db",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    ShippingTime = table.Column<string>(type: "text", nullable: true),
                    ReturnPolicy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_details", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_product_details_products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "catalog_db",
                        principalTable: "products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "product_images",
                schema: "catalog_db",
                columns: table => new
                {
                    ImageId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Url = table.Column<string>(type: "text", nullable: false),
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_images", x => x.ImageId);
                    table.ForeignKey(
                        name: "FK_product_images_products_ProductId",
                        column: x => x.ProductId,
                        principalSchema: "catalog_db",
                        principalTable: "products",
                        principalColumn: "ProductId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "catalog_db",
                table: "categories",
                columns: new[] { "CategoryId", "Name" },
                values: new object[,]
                {
                    { 1, "Electronics" },
                    { 2, "Books" },
                    { 3, "Gadgets" }
                });

            migrationBuilder.InsertData(
                schema: "orders_db",
                table: "customers",
                columns: new[] { "CustomerId", "CreatedAt", "Email", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 10, 15, 0, 0, 0, 0, DateTimeKind.Utc), "alice@mail.com", "Alice Johnson" },
                    { 2, new DateTime(2024, 10, 16, 0, 0, 0, 0, DateTimeKind.Utc), "bob@mail.com", "Bob Williams" }
                });

            migrationBuilder.InsertData(
                schema: "catalog_db",
                table: "suppliers",
                columns: new[] { "SupplierId", "Country", "Name", "Rating" },
                values: new object[,]
                {
                    { 1, "USA", "Tech Global", 4.5m },
                    { 2, "UK", "Book World", 4.8m }
                });

            migrationBuilder.InsertData(
                schema: "orders_db",
                table: "orders",
                columns: new[] { "OrderId", "CreatedAt", "CustomerId", "Status", "SupplierId" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 10, 17, 0, 0, 0, 0, DateTimeKind.Utc), 1, "Completed", 1 },
                    { 2, new DateTime(2024, 10, 18, 0, 0, 0, 0, DateTimeKind.Utc), 2, "Pending", 2 }
                });

            migrationBuilder.InsertData(
                schema: "catalog_db",
                table: "products",
                columns: new[] { "ProductId", "Name", "Price", "StockQuantity", "SupplierId" },
                values: new object[,]
                {
                    { 1, "Laptop X1", 999.99m, 10, 1 },
                    { 2, "EF Core Guide", 49.50m, 50, 2 },
                    { 3, "Smartwatch Z", 199.00m, 25, 1 }
                });

            migrationBuilder.InsertData(
                schema: "orders_db",
                table: "order_items",
                columns: new[] { "OrderId", "ProductId", "Quantity", "UnitPrice" },
                values: new object[,]
                {
                    { 1, 1, 1, 999.99m },
                    { 1, 3, 2, 199.00m },
                    { 2, 2, 5, 49.50m }
                });

            migrationBuilder.InsertData(
                schema: "orders_db",
                table: "payments",
                columns: new[] { "PaymentId", "Amount", "OrderId", "PaidAt" },
                values: new object[] { 1, 1397.99m, 1, new DateTime(2024, 10, 17, 10, 30, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                schema: "catalog_db",
                table: "product_categories",
                columns: new[] { "CategoryId", "ProductId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 3, 1 },
                    { 2, 2 },
                    { 1, 3 },
                    { 3, 3 }
                });

            migrationBuilder.InsertData(
                schema: "catalog_db",
                table: "product_details",
                columns: new[] { "ProductId", "Description", "ReturnPolicy", "ShippingTime" },
                values: new object[,]
                {
                    { 1, "Powerful laptop for development.", "30 days", "2-3 Days" },
                    { 2, "Complete guide to Entity Framework Core.", "15 days", "1-2 Days" },
                    { 3, "Smartwatch with health monitoring.", "30 days", "2-3 Days" }
                });

            migrationBuilder.InsertData(
                schema: "catalog_db",
                table: "product_images",
                columns: new[] { "ImageId", "ProductId", "Url" },
                values: new object[,]
                {
                    { 1, 1, "/img/laptop-x1.jpg" },
                    { 2, 1, "/img/laptop-x1-side.jpg" },
                    { 3, 3, "/img/smartwatch-z.jpg" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_order_items_ProductId",
                schema: "orders_db",
                table: "order_items",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_CustomerId",
                schema: "orders_db",
                table: "orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_orders_SupplierId",
                schema: "orders_db",
                table: "orders",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_payments_OrderId",
                schema: "orders_db",
                table: "payments",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_categories_CategoryId",
                schema: "catalog_db",
                table: "product_categories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_product_images_ProductId",
                schema: "catalog_db",
                table: "product_images",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_products_SupplierId",
                schema: "catalog_db",
                table: "products",
                column: "SupplierId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_items",
                schema: "orders_db");

            migrationBuilder.DropTable(
                name: "payments",
                schema: "orders_db");

            migrationBuilder.DropTable(
                name: "product_categories",
                schema: "catalog_db");

            migrationBuilder.DropTable(
                name: "product_details",
                schema: "catalog_db");

            migrationBuilder.DropTable(
                name: "product_images",
                schema: "catalog_db");

            migrationBuilder.DropTable(
                name: "orders",
                schema: "orders_db");

            migrationBuilder.DropTable(
                name: "categories",
                schema: "catalog_db");

            migrationBuilder.DropTable(
                name: "products",
                schema: "catalog_db");

            migrationBuilder.DropTable(
                name: "customers",
                schema: "orders_db");

            migrationBuilder.DropTable(
                name: "suppliers",
                schema: "catalog_db");
        }
    }
}
