using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend_Prog4.Migrations
{
    /// <inheritdoc />
    public partial class AddTallesADb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Talles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Talles", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Talles_Nombre",
                table: "Talles",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Talles");
        }
    }
}
