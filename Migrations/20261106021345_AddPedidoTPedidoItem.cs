using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_Prog4.Migrations
{
    /// <inheritdoc />
    public partial class AddPedidoTPedidoItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_EstadoPedidos_EstadoPedidoId",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductoTalle_Productos_ProductoId",
                table: "ProductoTalle");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductoTalle_Talles_TalleId",
                table: "ProductoTalle");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_EstadoPedidoId",
                table: "Pedidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductoTalle",
                table: "ProductoTalle");

            migrationBuilder.DropColumn(
                name: "EstadoPedidoId",
                table: "Pedidos");

            migrationBuilder.RenameTable(
                name: "ProductoTalle",
                newName: "ProductosTalles");

            migrationBuilder.RenameIndex(
                name: "IX_ProductoTalle_TalleId",
                table: "ProductosTalles",
                newName: "IX_ProductosTalles_TalleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductosTalles",
                table: "ProductosTalles",
                columns: new[] { "ProductoId", "TalleId" });

            migrationBuilder.CreateTable(
                name: "PedidoItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    PrecioUnitario = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PedidoId = table.Column<int>(type: "int", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    TalleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PedidoItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PedidoItems_Pedidos_PedidoId",
                        column: x => x.PedidoId,
                        principalTable: "Pedidos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PedidoItems_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PedidoItems_Talles_TalleId",
                        column: x => x.TalleId,
                        principalTable: "Talles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_IdEstado",
                table: "Pedidos",
                column: "IdEstado");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItems_PedidoId",
                table: "PedidoItems",
                column: "PedidoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItems_ProductoId",
                table: "PedidoItems",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_PedidoItems_TalleId",
                table: "PedidoItems",
                column: "TalleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_EstadoPedidos_IdEstado",
                table: "Pedidos",
                column: "IdEstado",
                principalTable: "EstadoPedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosTalles_Productos_ProductoId",
                table: "ProductosTalles",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductosTalles_Talles_TalleId",
                table: "ProductosTalles",
                column: "TalleId",
                principalTable: "Talles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_EstadoPedidos_IdEstado",
                table: "Pedidos");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosTalles_Productos_ProductoId",
                table: "ProductosTalles");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductosTalles_Talles_TalleId",
                table: "ProductosTalles");

            migrationBuilder.DropTable(
                name: "PedidoItems");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_IdEstado",
                table: "Pedidos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductosTalles",
                table: "ProductosTalles");

            migrationBuilder.RenameTable(
                name: "ProductosTalles",
                newName: "ProductoTalle");

            migrationBuilder.RenameIndex(
                name: "IX_ProductosTalles_TalleId",
                table: "ProductoTalle",
                newName: "IX_ProductoTalle_TalleId");

            migrationBuilder.AddColumn<int>(
                name: "EstadoPedidoId",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductoTalle",
                table: "ProductoTalle",
                columns: new[] { "ProductoId", "TalleId" });

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_EstadoPedidoId",
                table: "Pedidos",
                column: "EstadoPedidoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_EstadoPedidos_EstadoPedidoId",
                table: "Pedidos",
                column: "EstadoPedidoId",
                principalTable: "EstadoPedidos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoTalle_Productos_ProductoId",
                table: "ProductoTalle",
                column: "ProductoId",
                principalTable: "Productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductoTalle_Talles_TalleId",
                table: "ProductoTalle",
                column: "TalleId",
                principalTable: "Talles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
