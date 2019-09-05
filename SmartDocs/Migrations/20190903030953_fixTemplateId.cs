using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartDocs.Migrations
{
    public partial class fixTemplateId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Templates_TemplateId1",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_TemplateId1",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "TemplateId1",
                table: "Documents");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "Documents",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TemplateId",
                table: "Documents",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Templates_TemplateId",
                table: "Documents",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "TemplateId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Templates_TemplateId",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_TemplateId",
                table: "Documents");

            migrationBuilder.AlterColumn<string>(
                name: "TemplateId",
                table: "Documents",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "TemplateId1",
                table: "Documents",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TemplateId1",
                table: "Documents",
                column: "TemplateId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Templates_TemplateId1",
                table: "Documents",
                column: "TemplateId1",
                principalTable: "Templates",
                principalColumn: "TemplateId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
