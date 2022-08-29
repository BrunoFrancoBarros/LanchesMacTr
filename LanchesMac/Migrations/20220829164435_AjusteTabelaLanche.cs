using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LanchesMac.Migrations
{
    public partial class AjusteTabelaLanche : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagemThumbnaiUrl",
                table: "Lanches",
                newName: "ImagemThumbnailUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagemThumbnailUrl",
                table: "Lanches",
                newName: "ImagemThumbnaiUrl");
        }
    }
}
