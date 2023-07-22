using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jflutter.Migrations
{
    public partial class fix_question : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Users_User_id",
                table: "Question");

            migrationBuilder.DropIndex(
                name: "IX_Question_User_id",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "User_id",
                table: "Question");

            migrationBuilder.AddColumn<int>(
                name: "Module_id",
                table: "Question",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "QuestionUser",
                columns: table => new
                {
                    Questions_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Users_id = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuestionUser", x => new { x.Questions_id, x.Users_id });
                    table.ForeignKey(
                        name: "FK_QuestionUser_Question_Questions_id",
                        column: x => x.Questions_id,
                        principalTable: "Question",
                        principalColumn: "_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuestionUser_Users_Users_id",
                        column: x => x.Users_id,
                        principalTable: "Users",
                        principalColumn: "_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Question_Module_id",
                table: "Question",
                column: "Module_id");

            migrationBuilder.CreateIndex(
                name: "IX_QuestionUser_Users_id",
                table: "QuestionUser",
                column: "Users_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Module_Module_id",
                table: "Question",
                column: "Module_id",
                principalTable: "Module",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Question_Module_Module_id",
                table: "Question");

            migrationBuilder.DropTable(
                name: "QuestionUser");

            migrationBuilder.DropIndex(
                name: "IX_Question_Module_id",
                table: "Question");

            migrationBuilder.DropColumn(
                name: "Module_id",
                table: "Question");

            migrationBuilder.AddColumn<int>(
                name: "User_id",
                table: "Question",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Question_User_id",
                table: "Question",
                column: "User_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Users_User_id",
                table: "Question",
                column: "User_id",
                principalTable: "Users",
                principalColumn: "_id");
        }
    }
}
