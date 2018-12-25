using Microsoft.EntityFrameworkCore.Migrations;

namespace TwentyFirst.Data.Migrations
{
    public partial class CategoryOrderNotNull : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "Categories",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Order",
                table: "Categories",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
