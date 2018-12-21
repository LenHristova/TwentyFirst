using Microsoft.EntityFrameworkCore.Migrations;

namespace TwentyFirst.Data.Migrations
{
    public partial class SubscriberConfirmationCode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConfirmationCode",
                table: "Subscribers",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsConfirmed",
                table: "Subscribers",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmationCode",
                table: "Subscribers");

            migrationBuilder.DropColumn(
                name: "IsConfirmed",
                table: "Subscribers");
        }
    }
}
