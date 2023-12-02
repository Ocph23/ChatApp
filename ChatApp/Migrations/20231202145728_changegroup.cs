using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Migrations
{
    /// <inheritdoc />
    public partial class changegroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AnggotaId",
                table: "AnggotaGroup",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AnggotaGroup_AnggotaId",
                table: "AnggotaGroup",
                column: "AnggotaId");

            migrationBuilder.AddForeignKey(
                name: "FK_AnggotaGroup_AspNetUsers_AnggotaId",
                table: "AnggotaGroup",
                column: "AnggotaId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnggotaGroup_AspNetUsers_AnggotaId",
                table: "AnggotaGroup");

            migrationBuilder.DropIndex(
                name: "IX_AnggotaGroup_AnggotaId",
                table: "AnggotaGroup");

            migrationBuilder.DropColumn(
                name: "AnggotaId",
                table: "AnggotaGroup");
        }
    }
}
