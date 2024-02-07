using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inmobiliaria.Migrations
{
    /// <inheritdoc />
    public partial class newColumnPropietario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "inmobiliaria_id",
                table: "Propietarios",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Propietarios_inmobiliaria_id",
                table: "Propietarios",
                column: "inmobiliaria_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Propietarios_Inmobiliaria_inmobiliaria_id",
                table: "Propietarios",
                column: "inmobiliaria_id",
                principalTable: "Inmobiliaria",
                principalColumn: "inmobiliaria_id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Propietarios_Inmobiliaria_inmobiliaria_id",
                table: "Propietarios");

            migrationBuilder.DropIndex(
                name: "IX_Propietarios_inmobiliaria_id",
                table: "Propietarios");

            migrationBuilder.DropColumn(
                name: "inmobiliaria_id",
                table: "Propietarios");
        }
    }
}
