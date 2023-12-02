using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChatApp.Migrations
{
    /// <inheritdoc />
    public partial class message : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnggotaGroup_Group_GroupId",
                table: "AnggotaGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_Pertemanan_AspNetUsers_TemanId",
                table: "Pertemanan");

            migrationBuilder.DropForeignKey(
                name: "FK_Pertemanan_AspNetUsers_UserId",
                table: "Pertemanan");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Pertemanan",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TemanId",
                table: "Pertemanan",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "AnggotaGroup",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "PesanGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PesanGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PesanPrivat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PenerimaId = table.Column<string>(type: "text", nullable: true),
                    Text = table.Column<string>(type: "text", nullable: true),
                    UrlFile = table.Column<string>(type: "text", nullable: true),
                    MessageType = table.Column<int>(type: "integer", nullable: false),
                    PengirimId = table.Column<string>(type: "text", nullable: true),
                    Tanggal = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PesanPrivat", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_AnggotaGroup_Group_GroupId",
                table: "AnggotaGroup",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pertemanan_AspNetUsers_TemanId",
                table: "Pertemanan",
                column: "TemanId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pertemanan_AspNetUsers_UserId",
                table: "Pertemanan",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AnggotaGroup_Group_GroupId",
                table: "AnggotaGroup");

            migrationBuilder.DropForeignKey(
                name: "FK_Pertemanan_AspNetUsers_TemanId",
                table: "Pertemanan");

            migrationBuilder.DropForeignKey(
                name: "FK_Pertemanan_AspNetUsers_UserId",
                table: "Pertemanan");

            migrationBuilder.DropTable(
                name: "PesanGroup");

            migrationBuilder.DropTable(
                name: "PesanPrivat");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Pertemanan",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "TemanId",
                table: "Pertemanan",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<int>(
                name: "GroupId",
                table: "AnggotaGroup",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_AnggotaGroup_Group_GroupId",
                table: "AnggotaGroup",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pertemanan_AspNetUsers_TemanId",
                table: "Pertemanan",
                column: "TemanId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pertemanan_AspNetUsers_UserId",
                table: "Pertemanan",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
