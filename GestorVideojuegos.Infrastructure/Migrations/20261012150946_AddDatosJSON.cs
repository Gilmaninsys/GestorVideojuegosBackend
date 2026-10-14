using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestorVideojuegos.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddDatosJSON : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DatosJSON",
                table: "TierLists",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DatosJSON",
                table: "TierLists");
        }
    }
}
