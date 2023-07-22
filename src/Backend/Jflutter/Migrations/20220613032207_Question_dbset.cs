using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jflutter.Migrations
{
    public partial class Question_dbset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Question_Question_id",
                table: "Answer");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Lectures_lecture_id",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Modules_Module_id",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUser_Question_Questions_id",
                table: "QuestionUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Question",
                table: "Question");

            migrationBuilder.RenameTable(
                name: "Question",
                newName: "Questions");

            migrationBuilder.RenameIndex(
                name: "IX_Question_Module_id",
                table: "Questions",
                newName: "IX_Questions_Module_id");

            migrationBuilder.RenameIndex(
                name: "IX_Question_lecture_id",
                table: "Questions",
                newName: "IX_Questions_lecture_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Questions",
                table: "Questions",
                column: "_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Questions_Question_id",
                table: "Answer",
                column: "Question_id",
                principalTable: "Questions",
                principalColumn: "_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Lectures_lecture_id",
                table: "Questions",
                column: "lecture_id",
                principalTable: "Lectures",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Modules_Module_id",
                table: "Questions",
                column: "Module_id",
                principalTable: "Modules",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionUser_Questions_Questions_id",
                table: "QuestionUser",
                column: "Questions_id",
                principalTable: "Questions",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Answer_Questions_Question_id",
                table: "Answer");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Lectures_lecture_id",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Modules_Module_id",
                table: "Questions");

            migrationBuilder.DropForeignKey(
                name: "FK_QuestionUser_Questions_Questions_id",
                table: "QuestionUser");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Questions",
                table: "Questions");

            migrationBuilder.RenameTable(
                name: "Questions",
                newName: "Question");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_Module_id",
                table: "Question",
                newName: "IX_Question_Module_id");

            migrationBuilder.RenameIndex(
                name: "IX_Questions_lecture_id",
                table: "Question",
                newName: "IX_Question_lecture_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Question",
                table: "Question",
                column: "_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Answer_Question_Question_id",
                table: "Answer",
                column: "Question_id",
                principalTable: "Question",
                principalColumn: "_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Lectures_lecture_id",
                table: "Question",
                column: "lecture_id",
                principalTable: "Lectures",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Modules_Module_id",
                table: "Question",
                column: "Module_id",
                principalTable: "Modules",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_QuestionUser_Question_Questions_id",
                table: "QuestionUser",
                column: "Questions_id",
                principalTable: "Question",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
