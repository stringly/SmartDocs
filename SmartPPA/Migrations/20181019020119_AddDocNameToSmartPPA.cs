using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartPPA.Migrations
{
    public partial class AddDocNameToSmartPPA : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DocumentName",
                table: "PPAs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentName",
                table: "PPAs");
        }
    }
}
