using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyShop.BasicImpl.Migrations
{
    public partial class updateUserTableColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserState",
                table: "t_User_User",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "t_User_User",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserState",
                table: "t_User_User");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "t_User_User");
        }
    }
}
