using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EasyShop.BasicImpl.Migrations
{
    public partial class createDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "t_Table_Root",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    RootName = table.Column<string>(maxLength: 50, nullable: false),
                    RootCode = table.Column<string>(maxLength: 50, nullable: false),
                    Remark = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_Table_Root", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "t_User_Role",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    RoleName = table.Column<string>(maxLength: 50, nullable: false),
                    RoleCode = table.Column<string>(maxLength: 50, nullable: false),
                    Remark = table.Column<string>(maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_User_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "t_User_RoleRoot",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UserRoleId = table.Column<string>(maxLength: 32, nullable: false),
                    RootId = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_User_RoleRoot", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "t_User_User",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UserName = table.Column<string>(maxLength: 50, nullable: false),
                    Phone = table.Column<string>(maxLength: 20, nullable: false),
                    Password = table.Column<string>(maxLength: 10, nullable: false),
                    Photo = table.Column<string>(maxLength: 200, nullable: true),
                    QQNumber = table.Column<string>(maxLength: 50, nullable: true),
                    WeCharNumber = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_User_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "t_User_UserRole",
                columns: table => new
                {
                    Id = table.Column<string>(maxLength: 32, nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false),
                    UserId = table.Column<string>(maxLength: 32, nullable: false),
                    RoleId = table.Column<string>(maxLength: 32, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_t_User_UserRole", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "t_Table_Root");

            migrationBuilder.DropTable(
                name: "t_User_Role");

            migrationBuilder.DropTable(
                name: "t_User_RoleRoot");

            migrationBuilder.DropTable(
                name: "t_User_User");

            migrationBuilder.DropTable(
                name: "t_User_UserRole");
        }
    }
}
