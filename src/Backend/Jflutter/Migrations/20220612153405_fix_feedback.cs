using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jflutter.Migrations
{
    public partial class fix_feedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Student_id",
                table: "Feedbacks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_Student_id",
                table: "Feedbacks",
                column: "Student_id");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedbacks_Users_Student_id",
                table: "Feedbacks",
                column: "Student_id",
                principalTable: "Users",
                principalColumn: "_id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedbacks_Users_Student_id",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_Student_id",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "Student_id",
                table: "Feedbacks");
        }
    }
}
