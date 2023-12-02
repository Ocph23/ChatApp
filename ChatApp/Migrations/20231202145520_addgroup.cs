using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ChatApp.Migrations
{
    /// <inheritdoc />
    public partial class addgroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Group_GroupId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Group_GroupId1",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Group_AspNetUsers_PembuatId",
                table: "Group");

            migrationBuilder.DropForeignKey(
                name: "FK_Group_Kontak_ContactId",
                table: "Group");

            migrationBuilder.DropTable(
                name: "Teman");

            migrationBuilder.DropTable(
                name: "Kontak");

            migrationBuilder.DropIndex(
                name: "IX_Group_ContactId",
                table: "Group");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GroupId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_GroupId1",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ContactId",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "GroupId1",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "PembuatId",
                table: "Group",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateTable(
                name: "AnggotaGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Keanggotaan = table.Column<int>(type: "integer", nullable: false),
                    TanggalBergabung = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    GroupId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AnggotaGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AnggotaGroup_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Pertemanan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TemanId = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    TanggalBerteman = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pertemanan", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pertemanan_AspNetUsers_TemanId",
                        column: x => x.TemanId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Pertemanan_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AnggotaGroup_GroupId",
                table: "AnggotaGroup",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Pertemanan_TemanId",
                table: "Pertemanan",
                column: "TemanId");

            migrationBuilder.CreateIndex(
                name: "IX_Pertemanan_UserId",
                table: "Pertemanan",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_AspNetUsers_PembuatId",
                table: "Group",
                column: "PembuatId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Group_AspNetUsers_PembuatId",
                table: "Group");

            migrationBuilder.DropTable(
                name: "AnggotaGroup");

            migrationBuilder.DropTable(
                name: "Pertemanan");

            migrationBuilder.AlterColumn<string>(
                name: "PembuatId",
                table: "Group",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ContactId",
                table: "Group",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GroupId1",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Kontak",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kontak", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kontak_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Teman",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ContactId = table.Column<int>(type: "integer", nullable: true),
                    TanggalBerteman = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teman", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teman_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teman_Kontak_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Kontak",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Group_ContactId",
                table: "Group",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GroupId",
                table: "AspNetUsers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_GroupId1",
                table: "AspNetUsers",
                column: "GroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_Kontak_UserId",
                table: "Kontak",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Teman_ContactId",
                table: "Teman",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_Teman_UserId",
                table: "Teman",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Group_GroupId",
                table: "AspNetUsers",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Group_GroupId1",
                table: "AspNetUsers",
                column: "GroupId1",
                principalTable: "Group",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Group_AspNetUsers_PembuatId",
                table: "Group",
                column: "PembuatId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Group_Kontak_ContactId",
                table: "Group",
                column: "ContactId",
                principalTable: "Kontak",
                principalColumn: "Id");
        }
    }
}
