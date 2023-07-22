using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Jflutter.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InvitationCodes",
                columns: table => new
                {
                    _id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<long>(type: "INTEGER", nullable: false),
                    ValidityCount = table.Column<int>(type: "INTEGER", nullable: false),
                    Rule = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvitationCodes", x => x._id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    _id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PersonalCode = table.Column<long>(type: "INTEGER", nullable: false),
                    Rule = table.Column<int>(type: "INTEGER", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Firstname = table.Column<string>(type: "TEXT", nullable: false),
                    Lastname = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x._id);
                });

            migrationBuilder.CreateTable(
                name: "Module",
                columns: table => new
                {
                    _id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ModuleID = table.Column<long>(type: "INTEGER", nullable: false),
                    ModuleName = table.Column<string>(type: "TEXT", nullable: false),
                    ModuleDescription = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    User_id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Module", x => x._id);
                    table.ForeignKey(
                        name: "FK_Module_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "_id");
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    _id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FeedbackString = table.Column<string>(type: "TEXT", nullable: false),
                    Teacher_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Module_id = table.Column<int>(type: "INTEGER", nullable: false),
                    Flavour = table.Column<int>(type: "INTEGER", nullable: false),
                    Credibility = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x._id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Module_Module_id",
                        column: x => x.Module_id,
                        principalTable: "Module",
                        principalColumn: "_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Users_Teacher_id",
                        column: x => x.Teacher_id,
                        principalTable: "Users",
                        principalColumn: "_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Lecture",
                columns: table => new
                {
                    _id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LectureMaterial = table.Column<string>(type: "TEXT", nullable: false),
                    SecretCode = table.Column<long>(type: "INTEGER", nullable: false),
                    LectureDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Module_id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lecture", x => x._id);
                    table.ForeignKey(
                        name: "FK_Lecture_Module_Module_id",
                        column: x => x.Module_id,
                        principalTable: "Module",
                        principalColumn: "_id");
                });

            migrationBuilder.CreateTable(
                name: "Question",
                columns: table => new
                {
                    _id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    question = table.Column<string>(type: "TEXT", nullable: false),
                    lecture_id = table.Column<int>(type: "INTEGER", nullable: false),
                    User_id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Question", x => x._id);
                    table.ForeignKey(
                        name: "FK_Question_Lecture_lecture_id",
                        column: x => x.lecture_id,
                        principalTable: "Lecture",
                        principalColumn: "_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Question_Users_User_id",
                        column: x => x.User_id,
                        principalTable: "Users",
                        principalColumn: "_id");
                });

            migrationBuilder.CreateTable(
                name: "Answer",
                columns: table => new
                {
                    _id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Answear = table.Column<string>(type: "TEXT", nullable: false),
                    Question_id = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Answer", x => x._id);
                    table.ForeignKey(
                        name: "FK_Answer_Question_Question_id",
                        column: x => x.Question_id,
                        principalTable: "Question",
                        principalColumn: "_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Answer_Question_id",
                table: "Answer",
                column: "Question_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_Module_id",
                table: "Feedbacks",
                column: "Module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_Teacher_id",
                table: "Feedbacks",
                column: "Teacher_id");

            migrationBuilder.CreateIndex(
                name: "IX_Lecture_Module_id",
                table: "Lecture",
                column: "Module_id");

            migrationBuilder.CreateIndex(
                name: "IX_Module_User_id",
                table: "Module",
                column: "User_id");

            migrationBuilder.CreateIndex(
                name: "IX_Question_lecture_id",
                table: "Question",
                column: "lecture_id");

            migrationBuilder.CreateIndex(
                name: "IX_Question_User_id",
                table: "Question",
                column: "User_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Answer");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "InvitationCodes");

            migrationBuilder.DropTable(
                name: "Question");

            migrationBuilder.DropTable(
                name: "Lecture");

            migrationBuilder.DropTable(
                name: "Module");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
