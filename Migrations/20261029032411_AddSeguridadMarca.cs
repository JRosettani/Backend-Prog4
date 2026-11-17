using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_Prog4.Migrations
{
    /// <inheritdoc />
    public partial class AddSeguridadMarca : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MyProperty",
                table: "Marcas");

            migrationBuilder.AddColumn<int>(
                name: "EstadoPedidoId1",
                table: "Pedidos",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Nombre",
                table: "Marcas",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Pedidos_EstadoPedidoId1",
                table: "Pedidos",
                column: "EstadoPedidoId1");

            migrationBuilder.CreateIndex(
                name: "IX_Marcas_Nombre",
                table: "Marcas",
                column: "Nombre",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pedidos_EstadoPedidos_EstadoPedidoId1",
                table: "Pedidos",
                column: "EstadoPedidoId1",
                principalTable: "EstadoPedidos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pedidos_EstadoPedidos_EstadoPedidoId1",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Pedidos_EstadoPedidoId1",
                table: "Pedidos");

            migrationBuilder.DropIndex(
                name: "IX_Marcas_Nombre",
                table: "Marcas");

            migrationBuilder.DropColumn(
                name: "EstadoPedidoId1",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "Nombre",
                table: "Marcas");

            migrationBuilder.AddColumn<string>(
                name: "MyProperty",
                table: "Marcas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
