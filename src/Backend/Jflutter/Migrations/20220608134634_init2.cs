using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jflutter.Migrations
{
    public partial class init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Module_Users_User_id",
                table: "Module");

            migrationBuilder.DropIndex(
                name: "IX_Module_User_id",
                table: "Module");

            migrationBuilder.DropColumn(
                name: "User_id",
                table: "Module");

            migrationBuilder.CreateTable(
                name: "ModuleUser",
                columns: table => new
                {
                    Modules_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Students_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModuleUser", x => new { x.Modules_id, x.Students_id });
                    table.ForeignKey(
                        name: "FK_ModuleUser_Module_Modules_id",
                        column: x => x.Modules_id,
                        principalTable: "Module",
                        principalColumn: "_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ModuleUser_Users_Students_id",
                        column: x => x.Students_id,
                        principalTable: "Users",
                        principalColumn: "_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ModuleUser_Students_id",
                table: "ModuleUser",
                column: "Students_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModuleUser");

            migrationBuilder.AddColumn<int>(
                name: "User_id",
                table: "Module",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Module_User_id",
                table: "Module",
                column: "User_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Module_Users_User_id",
                table: "Module",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "_id");
        }
    }
}
