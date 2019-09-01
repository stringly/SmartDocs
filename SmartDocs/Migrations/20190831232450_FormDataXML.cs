using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartDocs.Migrations
{
    public partial class FormDataXML : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PPAs_Jobs_JobId",
                table: "PPAs");

            migrationBuilder.DropForeignKey(
                name: "FK_PPAs_Users_OwnerUserId",
                table: "PPAs");

            migrationBuilder.DropForeignKey(
                name: "FK_PPAs_Templates_TemplateId",
                table: "PPAs");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "PPAs",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OwnerUserId",
                table: "PPAs",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "JobId",
                table: "PPAs",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FormData",
                table: "PPAs",
                type: "xml",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PPAs_Jobs_JobId",
                table: "PPAs",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PPAs_Users_OwnerUserId",
                table: "PPAs",
                column: "OwnerUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PPAs_Templates_TemplateId",
                table: "PPAs",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "TemplateId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PPAs_Jobs_JobId",
                table: "PPAs");

            migrationBuilder.DropForeignKey(
                name: "FK_PPAs_Users_OwnerUserId",
                table: "PPAs");

            migrationBuilder.DropForeignKey(
                name: "FK_PPAs_Templates_TemplateId",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "FormData",
                table: "PPAs");

            migrationBuilder.AlterColumn<int>(
                name: "TemplateId",
                table: "PPAs",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "OwnerUserId",
                table: "PPAs",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "JobId",
                table: "PPAs",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_PPAs_Jobs_JobId",
                table: "PPAs",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "JobId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PPAs_Users_OwnerUserId",
                table: "PPAs",
                column: "OwnerUserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PPAs_Templates_TemplateId",
                table: "PPAs",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "TemplateId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
