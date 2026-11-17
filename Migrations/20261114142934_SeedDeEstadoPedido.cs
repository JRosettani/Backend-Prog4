using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Backend_Prog4.Migrations
{
    /// <inheritdoc />
    public partial class SeedDeEstadoPedido : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "EstadoPedidos",
                columns: new[] { "Id", "Nombre" },
                values: new object[,]
                {
                    { 1, "Pendiente" },
                    { 2, "Cancelado" },
                    { 3, "Confirmado" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "EstadoPedidos",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "EstadoPedidos",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "EstadoPedidos",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
