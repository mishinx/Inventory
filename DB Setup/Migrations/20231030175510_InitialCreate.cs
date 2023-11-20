using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace DB.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "administrators",
                columns: table => new
                {
                    admin_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    company_name = table.Column<string>(type: "text", nullable: false),
                    email_address = table.Column<string>(type: "text", nullable: false),
                    admin_password = table.Column<string>(type: "text", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administrators", x => x.admin_id);
                });

            migrationBuilder.CreateTable(
                name: "warehouses",
                columns: table => new
                {
                    warehouse_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    addres = table.Column<string>(type: "text", nullable: false),
                    admin_id_ref = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_warehouses", x => x.warehouse_id);
                    table.ForeignKey(
                        name: "FK_warehouses_administrators_admin_id_ref",
                        column: x => x.admin_id_ref,
                        principalTable: "administrators",
                        principalColumn: "admin_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "goods",
                columns: table => new
                {
                    goods_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    category = table.Column<string>(type: "text", nullable: false),
                    subcategory = table.Column<string>(type: "text", nullable: false),
                    short_description = table.Column<string>(type: "text", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    warehouse_id_ref = table.Column<int>(type: "integer", nullable: false),
                    photo = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_goods", x => x.goods_id);
                    table.ForeignKey(
                        name: "FK_goods_warehouses_warehouse_id_ref",
                        column: x => x.warehouse_id_ref,
                        principalTable: "warehouses",
                        principalColumn: "warehouse_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "operators",
                columns: table => new
                {
                    operator_id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    email_address = table.Column<string>(type: "text", nullable: false),
                    operator_password = table.Column<string>(type: "text", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: false),
                    phone_number = table.Column<string>(type: "text", nullable: false),
                    warehouse_id_ref = table.Column<int>(type: "integer", nullable: false),
                    admin_id_ref = table.Column<int>(type: "integer", nullable: false),
                    photo = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operators", x => x.operator_id);
                    table.ForeignKey(
                        name: "FK_operators_administrators_admin_id_ref",
                        column: x => x.admin_id_ref,
                        principalTable: "administrators",
                        principalColumn: "admin_id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_operators_warehouses_warehouse_id_ref",
                        column: x => x.warehouse_id_ref,
                        principalTable: "warehouses",
                        principalColumn: "warehouse_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_goods_warehouse_id_ref",
                table: "goods",
                column: "warehouse_id_ref");

            migrationBuilder.CreateIndex(
                name: "IX_operators_admin_id_ref",
                table: "operators",
                column: "admin_id_ref");

            migrationBuilder.CreateIndex(
                name: "IX_operators_warehouse_id_ref",
                table: "operators",
                column: "warehouse_id_ref");

            migrationBuilder.CreateIndex(
                name: "IX_warehouses_admin_id_ref",
                table: "warehouses",
                column: "admin_id_ref");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "goods");

            migrationBuilder.DropTable(
                name: "operators");

            migrationBuilder.DropTable(
                name: "warehouses");

            migrationBuilder.DropTable(
                name: "administrators");
        }
    }
}
