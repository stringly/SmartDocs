using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartDocs.Migrations
{
    public partial class SmartDocument : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PPAs");

            migrationBuilder.RenameColumn(
                name: "DocumentName",
                table: "Templates",
                newName: "Name");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Templates",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Documents",
                columns: table => new
                {
                    DocumentId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AuthorUserId = table.Column<int>(nullable: false),
                    TemplateId = table.Column<string>(nullable: true),
                    TemplateId1 = table.Column<int>(nullable: true),
                    Type = table.Column<string>(nullable: false),
                    Created = table.Column<DateTime>(nullable: false),
                    Edited = table.Column<DateTime>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    FormData = table.Column<string>(type: "xml", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.DocumentId);
                    table.ForeignKey(
                        name: "FK_Documents_Users_AuthorUserId",
                        column: x => x.AuthorUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_Templates_TemplateId1",
                        column: x => x.TemplateId1,
                        principalTable: "Templates",
                        principalColumn: "TemplateId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Documents_AuthorUserId",
                table: "Documents",
                column: "AuthorUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TemplateId1",
                table: "Documents",
                column: "TemplateId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Documents");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Templates");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Templates",
                newName: "DocumentName");

            migrationBuilder.CreateTable(
                name: "PPAs",
                columns: table => new
                {
                    PPAId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AssessmentComments = table.Column<string>(nullable: true),
                    CategoryScore_1 = table.Column<int>(nullable: true),
                    CategoryScore_2 = table.Column<int>(nullable: true),
                    CategoryScore_3 = table.Column<int>(nullable: true),
                    CategoryScore_4 = table.Column<int>(nullable: true),
                    CategoryScore_5 = table.Column<int>(nullable: true),
                    CategoryScore_6 = table.Column<int>(nullable: true),
                    Created = table.Column<DateTime>(nullable: false),
                    DepartmentDivision = table.Column<string>(nullable: true),
                    DepartmentDivisionCode = table.Column<string>(nullable: true),
                    DepartmentIdNumber = table.Column<string>(nullable: true),
                    DocumentName = table.Column<string>(nullable: true),
                    EmployeeFirstName = table.Column<string>(nullable: true),
                    EmployeeLastName = table.Column<string>(nullable: true),
                    EndDate = table.Column<DateTime>(nullable: false),
                    FormData = table.Column<string>(type: "xml", nullable: true),
                    JobId = table.Column<int>(nullable: false),
                    Modified = table.Column<DateTime>(nullable: false),
                    OwnerUserId = table.Column<int>(nullable: false),
                    PayrollIdNumber = table.Column<string>(nullable: true),
                    PositionNumber = table.Column<string>(nullable: true),
                    RecommendationComments = table.Column<string>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    SupervisedByEmployee = table.Column<string>(nullable: true),
                    TemplateId = table.Column<int>(nullable: false),
                    WorkplaceAddress = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PPAs", x => x.PPAId);
                    table.ForeignKey(
                        name: "FK_PPAs_Jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "Jobs",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PPAs_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PPAs_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "TemplateId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PPAs_JobId",
                table: "PPAs",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_PPAs_OwnerUserId",
                table: "PPAs",
                column: "OwnerUserId");

            migrationBuilder.CreateIndex(
                name: "IX_PPAs_TemplateId",
                table: "PPAs",
                column: "TemplateId");
        }
    }
}
