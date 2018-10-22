using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartDocs.Migrations
{
    public partial class EliminateFormXML : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FormData",
                table: "PPAs");

            migrationBuilder.AddColumn<string>(
                name: "AssessmentComments",
                table: "PPAs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryScore_1",
                table: "PPAs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryScore_2",
                table: "PPAs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryScore_3",
                table: "PPAs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryScore_4",
                table: "PPAs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryScore_5",
                table: "PPAs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CategoryScore_6",
                table: "PPAs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentDivision",
                table: "PPAs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentDivisionCode",
                table: "PPAs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DepartmentIdNumber",
                table: "PPAs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeFirstName",
                table: "PPAs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeLastName",
                table: "PPAs",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "PPAs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "PayrollIdNumber",
                table: "PPAs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PositionNumber",
                table: "PPAs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RecommendationComments",
                table: "PPAs",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "PPAs",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SupervisedByEmployee",
                table: "PPAs",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkplaceAddress",
                table: "PPAs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssessmentComments",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "CategoryScore_1",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "CategoryScore_2",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "CategoryScore_3",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "CategoryScore_4",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "CategoryScore_5",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "CategoryScore_6",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "DepartmentDivision",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "DepartmentDivisionCode",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "DepartmentIdNumber",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "EmployeeFirstName",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "EmployeeLastName",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "PayrollIdNumber",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "PositionNumber",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "RecommendationComments",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "SupervisedByEmployee",
                table: "PPAs");

            migrationBuilder.DropColumn(
                name: "WorkplaceAddress",
                table: "PPAs");

            migrationBuilder.AddColumn<string>(
                name: "FormData",
                table: "PPAs",
                type: "xml",
                nullable: true);
        }
    }
}
