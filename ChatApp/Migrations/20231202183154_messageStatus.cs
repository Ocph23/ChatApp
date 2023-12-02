using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Migrations
{
    /// <inheritdoc />
    public partial class messageStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "PesanPrivat",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "PesanGroup",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "PesanPrivat");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "PesanGroup");
        }
    }
}
