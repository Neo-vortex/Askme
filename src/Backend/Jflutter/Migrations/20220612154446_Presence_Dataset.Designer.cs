﻿// <auto-generated />
using System;
using Jflutter.Services.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Jflutter.Migrations
{
    [DbContext(typeof(DatabaseManager))]
    [Migration("20220612154446_Presence_Dataset")]
    partial class Presence_Dataset
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.5");

            modelBuilder.Entity("Jflutter.Entities.Answer", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Answear")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Question_id")
                        .HasColumnType("INTEGER");

                    b.HasKey("_id");

                    b.HasIndex("Question_id");

                    b.ToTable("Answer");
                });

            modelBuilder.Entity("Jflutter.Entities.Feedback", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Credibility")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FeedbackString")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Flavour")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Module_id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Student_id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Teacher_id")
                        .HasColumnType("INTEGER");

                    b.HasKey("_id");

                    b.HasIndex("Module_id");

                    b.HasIndex("Student_id");

                    b.HasIndex("Teacher_id");

                    b.ToTable("Feedbacks");
                });

            modelBuilder.Entity("Jflutter.Entities.InvitationCode", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("Code")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Rule")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ValidityCount")
                        .HasColumnType("INTEGER");

                    b.HasKey("_id");

                    b.ToTable("InvitationCodes");
                });

            modelBuilder.Entity("Jflutter.Entities.Lecture", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LectureDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("LectureMaterial")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("Module_id")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SecretCode")
                        .HasColumnType("INTEGER");

                    b.HasKey("_id");

                    b.HasIndex("Module_id");

                    b.ToTable("Lectures");
                });

            modelBuilder.Entity("Jflutter.Entities.Module", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModuleDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("ModuleID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ModuleName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("_id");

                    b.ToTable("Modules");
                });

            modelBuilder.Entity("Jflutter.Entities.Presence", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("LectureID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Student_id")
                        .HasColumnType("INTEGER");

                    b.HasKey("_id");

                    b.HasIndex("Student_id");

                    b.ToTable("Presences");
                });

            modelBuilder.Entity("Jflutter.Entities.Question", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Module_id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("lecture_id")
                        .HasColumnType("INTEGER");

                    b.Property<string>("question")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("_id");

                    b.HasIndex("Module_id");

                    b.HasIndex("lecture_id");

                    b.ToTable("Question");
                });

            modelBuilder.Entity("Jflutter.Entities.User", b =>
                {
                    b.Property<int>("_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Firstname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Lastname")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long>("PersonalCode")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Rule")
                        .HasColumnType("INTEGER");

                    b.HasKey("_id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ModuleUser", b =>
                {
                    b.Property<int>("Modules_id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Students_id")
                        .HasColumnType("INTEGER");

                    b.HasKey("Modules_id", "Students_id");

                    b.HasIndex("Students_id");

                    b.ToTable("ModuleUser");
                });

            modelBuilder.Entity("QuestionUser", b =>
                {
                    b.Property<int>("Questions_id")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Users_id")
                        .HasColumnType("INTEGER");

                    b.HasKey("Questions_id", "Users_id");

                    b.HasIndex("Users_id");

                    b.ToTable("QuestionUser");
                });

            modelBuilder.Entity("Jflutter.Entities.Answer", b =>
                {
                    b.HasOne("Jflutter.Entities.Question", null)
                        .WithMany("answers")
                        .HasForeignKey("Question_id");
                });

            modelBuilder.Entity("Jflutter.Entities.Feedback", b =>
                {
                    b.HasOne("Jflutter.Entities.Module", "Module")
                        .WithMany()
                        .HasForeignKey("Module_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Jflutter.Entities.User", "Student")
                        .WithMany()
                        .HasForeignKey("Student_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Jflutter.Entities.User", "Teacher")
                        .WithMany()
                        .HasForeignKey("Teacher_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Module");

                    b.Navigation("Student");

                    b.Navigation("Teacher");
                });

            modelBuilder.Entity("Jflutter.Entities.Lecture", b =>
                {
                    b.HasOne("Jflutter.Entities.Module", null)
                        .WithMany("Lectures")
                        .HasForeignKey("Module_id");
                });

            modelBuilder.Entity("Jflutter.Entities.Presence", b =>
                {
                    b.HasOne("Jflutter.Entities.User", "Student")
                        .WithMany()
                        .HasForeignKey("Student_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Student");
                });

            modelBuilder.Entity("Jflutter.Entities.Question", b =>
                {
                    b.HasOne("Jflutter.Entities.Module", "Module")
                        .WithMany()
                        .HasForeignKey("Module_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Jflutter.Entities.Lecture", "lecture")
                        .WithMany()
                        .HasForeignKey("lecture_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Module");

                    b.Navigation("lecture");
                });

            modelBuilder.Entity("ModuleUser", b =>
                {
                    b.HasOne("Jflutter.Entities.Module", null)
                        .WithMany()
                        .HasForeignKey("Modules_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Jflutter.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("Students_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("QuestionUser", b =>
                {
                    b.HasOne("Jflutter.Entities.Question", null)
                        .WithMany()
                        .HasForeignKey("Questions_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Jflutter.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("Users_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Jflutter.Entities.Module", b =>
                {
                    b.Navigation("Lectures");
                });

            modelBuilder.Entity("Jflutter.Entities.Question", b =>
                {
                    b.Navigation("answers");
                });
#pragma warning restore 612, 618
        }
    }
}
