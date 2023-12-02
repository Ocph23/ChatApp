using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Migrations
{
    /// <inheritdoc />
    public partial class messageGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MessageType",
                table: "PesanGroup",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PengirimId",
                table: "PesanGroup",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Tanggal",
                table: "PesanGroup",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "PesanGroup",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UrlFile",
                table: "PesanGroup",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MessageType",
                table: "PesanGroup");

            migrationBuilder.DropColumn(
                name: "PengirimId",
                table: "PesanGroup");

            migrationBuilder.DropColumn(
                name: "Tanggal",
                table: "PesanGroup");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "PesanGroup");

            migrationBuilder.DropColumn(
                name: "UrlFile",
                table: "PesanGroup");
        }
    }
}
