using Microsoft.EntityFrameworkCore.Migrations;

namespace Database.Migrations
{
    public partial class NewVer3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Vendor",
                table: "Products",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Vendor",
                table: "Products");
        }
    }
}
