using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace SalesApplication.Database.Migrations
{
    public partial class Default1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "salesmanagement");

            migrationBuilder.CreateTable(
                name: "Customer",
                schema: "salesmanagement",
                columns: table => new
                {
                    customer_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("customer_id", x => x.customer_id);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                schema: "salesmanagement",
                columns: table => new
                {
                    product_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_description = table.Column<string>(type: "text", nullable: true),
                    product_price = table.Column<double>(type: "double precision", nullable: false),
                    product_stock = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("product_id", x => x.product_id);
                });

            migrationBuilder.CreateTable(
                name: "Sale",
                schema: "salesmanagement",
                columns: table => new
                {
                    sale_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sale_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    client_id = table.Column<int>(type: "integer", nullable: false),
                    total_amount = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("sale_id", x => x.sale_id);
                    table.ForeignKey(
                        name: "FK_Sale_Customer_client_id",
                        column: x => x.client_id,
                        principalSchema: "salesmanagement",
                        principalTable: "Customer",
                        principalColumn: "customer_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoldProducts",
                schema: "salesmanagement",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    sale_id = table.Column<int>(type: "integer", nullable: false),
                    product_price = table.Column<double>(type: "double precision", nullable: false),
                    product_id = table.Column<int>(type: "integer", nullable: false),
                    product_amount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("id", x => x.id);
                    table.ForeignKey(
                        name: "FK_SoldProducts_Product_product_id",
                        column: x => x.product_id,
                        principalSchema: "salesmanagement",
                        principalTable: "Product",
                        principalColumn: "product_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoldProducts_Sale_sale_id",
                        column: x => x.sale_id,
                        principalSchema: "salesmanagement",
                        principalTable: "Sale",
                        principalColumn: "sale_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sale_client_id",
                schema: "salesmanagement",
                table: "Sale",
                column: "client_id");

            migrationBuilder.CreateIndex(
                name: "IX_SoldProducts_product_id",
                schema: "salesmanagement",
                table: "SoldProducts",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_SoldProducts_sale_id",
                schema: "salesmanagement",
                table: "SoldProducts",
                column: "sale_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoldProducts",
                schema: "salesmanagement");

            migrationBuilder.DropTable(
                name: "Product",
                schema: "salesmanagement");

            migrationBuilder.DropTable(
                name: "Sale",
                schema: "salesmanagement");

            migrationBuilder.DropTable(
                name: "Customer",
                schema: "salesmanagement");
        }
    }
}
