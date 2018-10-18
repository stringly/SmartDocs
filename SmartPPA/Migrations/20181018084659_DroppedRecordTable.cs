using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartPPA.Migrations
{
    public partial class DroppedRecordTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PPAs_Records_RecordDocumentId",
                table: "PPAs");

            migrationBuilder.DropTable(
                name: "Records");

            migrationBuilder.RenameColumn(
                name: "RecordDocumentId",
                table: "PPAs",
                newName: "OwnerUserId");

            migrationBuilder.RenameIndex(
                name: "IX_PPAs_RecordDocumentId",
                table: "PPAs",
                newName: "IX_PPAs_OwnerUserId");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "PPAs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "Modified",
                table: "PPAs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_PPAs_Users_OwnerUserId",
                table: "PPAs",
                column: "OwnerUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PPAs_Users_OwnerUserId",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "Modified",
                table: "PPAs");

            migrationBuilder.RenameColumn(
                name: "OwnerUserId",
                table: "PPAs",
                newName: "RecordDocumentId");

            migrationBuilder.RenameIndex(
                name: "IX_PPAs_OwnerUserId",
                table: "PPAs",
                newName: "IX_PPAs_RecordDocumentId");

            migrationBuilder.CreateTable(
                name: "Records",
                columns: table => new
                {
                    DocumentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTime>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Records", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Records_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Records_UserId",
                table: "Records",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PPAs_Records_RecordDocumentId",
                table: "PPAs",
                column: "RecordDocumentId",
                principalTable: "Records",
                principalColumn: "DocumentId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
