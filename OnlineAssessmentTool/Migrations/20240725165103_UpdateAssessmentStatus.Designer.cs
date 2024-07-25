﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using OnlineAssessmentTool.Data;

#nullable disable

namespace OnlineAssessmentTool.Migrations
{
    [DbContext(typeof(APIContext))]
    [Migration("20240725165103_UpdateAssessmentStatus")]
    partial class UpdateAssessmentStatus
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("OnlineAssessmentTool.Models.Assessment", b =>
                {
                    b.Property<int>("AssessmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AssessmentId"));

                    b.Property<string>("AssessmentName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("TotalScore")
                        .HasColumnType("integer");

                    b.HasKey("AssessmentId");

                    b.HasIndex("CreatedBy");

                    b.ToTable("Assessments");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.AssessmentScore", b =>
                {
                    b.Property<int>("AssessmentScoreId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("AssessmentScoreId"));

                    b.Property<int>("AvergeScore")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CalculatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ScheduledAssessmentId")
                        .HasColumnType("integer");

                    b.Property<int>("TraineeId")
                        .HasColumnType("integer");

                    b.HasKey("AssessmentScoreId");

                    b.ToTable("AssessmentScores");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.Batch", b =>
                {
                    b.Property<int>("batchid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("batchid"));

                    b.Property<string>("batchname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("batchid");

                    b.ToTable("batch");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.Permission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PermissionName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.Question", b =>
                {
                    b.Property<int>("QuestionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("QuestionId"));

                    b.Property<int>("AssessmentId")
                        .HasColumnType("integer");

                    b.Property<int>("CreatedBy")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Points")
                        .HasColumnType("integer");

                    b.Property<string>("QuestionText")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("QuestionType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("QuestionId");

                    b.HasIndex("AssessmentId");

                    b.HasIndex("CreatedBy");

                    b.ToTable("Questions");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.QuestionOption", b =>
                {
                    b.Property<int>("QuestionOptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("QuestionOptionId"));

                    b.Property<string>("CorrectAnswer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Option1")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Option2")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Option3")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Option4")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("QuestionId")
                        .HasColumnType("integer");

                    b.HasKey("QuestionOptionId");

                    b.HasIndex("QuestionId");

                    b.ToTable("QuestionOptions");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Roles", (string)null);
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.ScheduledAssessment", b =>
                {
                    b.Property<int>("ScheduledAssessmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ScheduledAssessmentId"));

                    b.Property<TimeSpan>("AssessmentDuration")
                        .HasColumnType("interval");

                    b.Property<int>("AssessmentId")
                        .HasColumnType("integer");

                    b.Property<int>("BatchId")
                        .HasColumnType("integer");

                    b.Property<bool>("CanDisplayResult")
                        .HasColumnType("boolean");

                    b.Property<bool>("CanRandomizeQuestion")
                        .HasColumnType("boolean");

                    b.Property<bool>("CanSubmitBeforeEnd")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("ScheduledDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("ScheduledAssessmentId");

                    b.ToTable("ScheduledAssessments");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.Trainee", b =>
                {
                    b.Property<int>("TraineeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TraineeId"));

                    b.Property<int>("BatchId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("JoinedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("TraineeId");

                    b.HasIndex("BatchId");

                    b.HasIndex("UserId");

                    b.ToTable("Trainees");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.TraineeAnswer", b =>
                {
                    b.Property<int>("TraineeAnswerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TraineeAnswerId"));

                    b.Property<string>("Answer")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsCorrect")
                        .HasColumnType("boolean");

                    b.Property<int>("QuestionId")
                        .HasColumnType("integer");

                    b.Property<int>("ScheduledAssessmentId")
                        .HasColumnType("integer");

                    b.Property<int>("Score")
                        .HasColumnType("integer");

                    b.Property<int>("TraineeId")
                        .HasColumnType("integer");

                    b.HasKey("TraineeAnswerId");

                    b.HasIndex("QuestionId");

                    b.ToTable("TraineeAnswers");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.Trainer", b =>
                {
                    b.Property<int>("TrainerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("TrainerId"));

                    b.Property<DateTime>("JoinedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Password")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("TrainerId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("Trainers", (string)null);
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.TrainerBatch", b =>
                {
                    b.Property<int>("Trainer_id")
                        .HasColumnType("integer");

                    b.Property<int>("Batch_id")
                        .HasColumnType("integer");

                    b.HasKey("Trainer_id", "Batch_id");

                    b.HasIndex("Batch_id");

                    b.ToTable("TrainerBatches");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.Users", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<Guid>("UUID")
                        .HasColumnType("uuid");

                    b.Property<int>("UserType")
                        .HasColumnType("integer");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("UserId");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("RolePermission", b =>
                {
                    b.Property<int>("PermissionId")
                        .HasColumnType("integer");

                    b.Property<int>("RoleId")
                        .HasColumnType("integer");

                    b.HasKey("PermissionId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("RolePermission");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.Assessment", b =>
                {
                    b.HasOne("OnlineAssessmentTool.Models.Trainer", "Trainer")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.Question", b =>
                {
                    b.HasOne("OnlineAssessmentTool.Models.Assessment", "Assessment")
                        .WithMany("Questions")
                        .HasForeignKey("AssessmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineAssessmentTool.Models.Trainer", "Trainer")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Assessment");

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.QuestionOption", b =>
                {
                    b.HasOne("OnlineAssessmentTool.Models.Question", "Question")
                        .WithMany("QuestionOptions")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.Trainee", b =>
                {
                    b.HasOne("OnlineAssessmentTool.Models.Batch", "Batch")
                        .WithMany("Trainees")
                        .HasForeignKey("BatchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineAssessmentTool.Models.Users", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Batch");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.TraineeAnswer", b =>
                {
                    b.HasOne("OnlineAssessmentTool.Models.Question", "Question")
                        .WithMany("TraineeAnswers")
                        .HasForeignKey("QuestionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Question");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.Trainer", b =>
                {
                    b.HasOne("OnlineAssessmentTool.Models.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineAssessmentTool.Models.Users", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.TrainerBatch", b =>
                {
                    b.HasOne("OnlineAssessmentTool.Models.Batch", "Batch")
                        .WithMany("TrainerBatch")
                        .HasForeignKey("Batch_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineAssessmentTool.Models.Trainer", "Trainer")
                        .WithMany("TrainerBatch")
                        .HasForeignKey("Trainer_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Batch");

                    b.Navigation("Trainer");
                });

            modelBuilder.Entity("RolePermission", b =>
                {
                    b.HasOne("OnlineAssessmentTool.Models.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("OnlineAssessmentTool.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.Assessment", b =>
                {
                    b.Navigation("Questions");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.Batch", b =>
                {
                    b.Navigation("Trainees");

                    b.Navigation("TrainerBatch");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.Question", b =>
                {
                    b.Navigation("QuestionOptions");

                    b.Navigation("TraineeAnswers");
                });

            modelBuilder.Entity("OnlineAssessmentTool.Models.Trainer", b =>
                {
                    b.Navigation("TrainerBatch");
                });
#pragma warning restore 612, 618
        }
    }
}
