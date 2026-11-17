using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_Prog4.Migrations
{
    /// <inheritdoc />
    public partial class addUsuarioArregladoConRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "esAdmin",
                table: "Usuarios");

            migrationBuilder.CreateTable(
                name: "Rol",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rol", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rol_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rol_UsuarioId",
                table: "Rol",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rol");

            migrationBuilder.AddColumn<bool>(
                name: "esAdmin",
                table: "Usuarios",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
