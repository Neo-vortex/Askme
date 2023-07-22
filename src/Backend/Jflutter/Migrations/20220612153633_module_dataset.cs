using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jflutter.Migrations
{
    public partial class module_dataset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Module_Module_id",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Lecture_Module_Module_id",
                table: "Lecture");

            migrationBuilder.DropForeignKey(
                name: "FK_ModuleUser_Module_Modules_id",
                table: "ModuleUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Lecture_lecture_id",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Module_Module_id",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Module",
                table: "Module");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lecture",
                table: "Lecture");

            migrationBuilder.RenameTable(
                name: "Module",
                newName: "Modules");

            migrationBuilder.RenameTable(
                name: "Lecture",
                newName: "Lectures");

            migrationBuilder.RenameIndex(
                name: "IX_Lecture_Module_id",
                table: "Lectures",
                newName: "IX_Lectures_Module_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Modules",
                table: "Modules",
                column: "_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lectures",
                table: "Lectures",
                column: "_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Modules_Module_id",
                table: "Feedbacks",
                column: "Module_id",
                principalTable: "Modules",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lectures_Modules_Module_id",
                table: "Lectures",
                column: "Module_id",
                principalTable: "Modules",
                principalColumn: "_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleUser_Modules_Modules_id",
                table: "ModuleUser",
                column: "Modules_id",
                principalTable: "Modules",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Modules_Module_id",
                table: "Feedbacks");

            migrationBuilder.DropForeignKey(
                name: "FK_Lectures_Modules_Module_id",
                table: "Lectures");

            migrationBuilder.DropForeignKey(
                name: "FK_ModuleUser_Modules_Modules_id",
                table: "ModuleUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Lectures_lecture_id",
                table: "Question");

            migrationBuilder.DropForeignKey(
                name: "FK_Question_Modules_Module_id",
                table: "Question");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Modules",
                table: "Modules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Lectures",
                table: "Lectures");

            migrationBuilder.RenameTable(
                name: "Modules",
                newName: "Module");

            migrationBuilder.RenameTable(
                name: "Lectures",
                newName: "Lecture");

            migrationBuilder.RenameIndex(
                name: "IX_Lectures_Module_id",
                table: "Lecture",
                newName: "IX_Lecture_Module_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Module",
                table: "Module",
                column: "_id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Lecture",
                table: "Lecture",
                column: "_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Module_Module_id",
                table: "Feedbacks",
                column: "Module_id",
                principalTable: "Module",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Lecture_Module_Module_id",
                table: "Lecture",
                column: "Module_id",
                principalTable: "Module",
                principalColumn: "_id");

            migrationBuilder.AddForeignKey(
                name: "FK_ModuleUser_Module_Modules_id",
                table: "ModuleUser",
                column: "Modules_id",
                principalTable: "Module",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Lecture_lecture_id",
                table: "Question",
                column: "lecture_id",
                principalTable: "Lecture",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Question_Module_Module_id",
                table: "Question",
                column: "Module_id",
                principalTable: "Module",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
