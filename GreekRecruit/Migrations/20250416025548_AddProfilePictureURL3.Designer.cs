﻿// <auto-generated />
using System;
using GreekRecruit.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GreekRecruit.Migrations
{
    [DbContext(typeof(SqlDataContext))]
    [Migration("20250416025548_AddProfilePictureURL3")]
    partial class AddProfilePictureURL3
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GreekRecruit.Models.AdminTask", b =>
                {
                    b.Property<int>("task_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("task_id"));

                    b.Property<DateTime?>("date_completed")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("date_created")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("due_date")
                        .HasColumnType("datetime2");

                    b.Property<bool>("is_completed")
                        .HasColumnType("bit");

                    b.Property<int>("organization_id")
                        .HasColumnType("int");

                    b.Property<string>("task_description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("user_id")
                        .HasColumnType("int");

                    b.HasKey("task_id");

                    b.ToTable("AdminTasks");
                });

            modelBuilder.Entity("GreekRecruit.Models.Comment", b =>
                {
                    b.Property<int>("comment_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("comment_id"));

                    b.Property<string>("comment_author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("comment_author_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("comment_dt")
                        .HasColumnType("datetime2");

                    b.Property<string>("comment_text")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("comment_type")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("pnm_id")
                        .HasColumnType("int");

                    b.HasKey("comment_id");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("GreekRecruit.Models.Event", b =>
                {
                    b.Property<int>("event_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("event_id"));

                    b.Property<string>("event_address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("event_datetime")
                        .HasColumnType("datetime2");

                    b.Property<string>("event_description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("event_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("organization_id")
                        .HasColumnType("int");

                    b.HasKey("event_id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("GreekRecruit.Models.EventAttendance", b =>
                {
                    b.Property<int>("attendance_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("attendance_id"));

                    b.Property<DateTime>("checked_in_at")
                        .HasColumnType("datetime2");

                    b.Property<int>("event_id")
                        .HasColumnType("int");

                    b.Property<int>("organization_id")
                        .HasColumnType("int");

                    b.Property<string>("pnm_fname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pnm_lname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pnm_schoolyear")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("attendance_id");

                    b.ToTable("EventsAttendance");
                });

            modelBuilder.Entity("GreekRecruit.Models.InterestForm", b =>
                {
                    b.Property<int>("form_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("form_id"));

                    b.Property<DateTime>("date_created")
                        .HasColumnType("datetime2");

                    b.Property<string>("form_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("organization_id")
                        .HasColumnType("int");

                    b.HasKey("form_id");

                    b.ToTable("InterestForms");
                });

            modelBuilder.Entity("GreekRecruit.Models.InterestFormSubmission", b =>
                {
                    b.Property<int>("submission_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("submission_id"));

                    b.Property<DateTime>("date_submitted")
                        .HasColumnType("datetime2");

                    b.Property<int>("form_id")
                        .HasColumnType("int");

                    b.Property<int>("organization_id")
                        .HasColumnType("int");

                    b.Property<string>("pnm_email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pnm_fname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("pnm_gpa")
                        .HasColumnType("float");

                    b.Property<string>("pnm_instagramhandle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pnm_lname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pnm_major")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pnm_membersknown")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pnm_phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("pnm_profilepicture")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("pnm_profilepictureurl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pnm_schoolyear")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("submission_id");

                    b.ToTable("InterestFormSubmissions");
                });

            modelBuilder.Entity("GreekRecruit.Models.Organization", b =>
                {
                    b.Property<int>("organization_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("organization_id"));

                    b.Property<string>("organization_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("organization_id");

                    b.ToTable("Organizations");
                });

            modelBuilder.Entity("GreekRecruit.Models.PNM", b =>
                {
                    b.Property<int>("pnm_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("pnm_id"));

                    b.Property<int>("organization_id")
                        .HasColumnType("int");

                    b.Property<string>("pnm_comments")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("pnm_dateadded")
                        .HasColumnType("datetime2");

                    b.Property<string>("pnm_email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pnm_fname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("pnm_gpa")
                        .HasColumnType("float");

                    b.Property<string>("pnm_instagramhandle")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pnm_lname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pnm_major")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pnm_phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("pnm_profilepicture")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("pnm_profilepictureurl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pnm_schoolyear")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pnm_semester")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("pnm_status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("pnm_id");

                    b.ToTable("PNMs");
                });

            modelBuilder.Entity("GreekRecruit.Models.PNMVoteSession", b =>
                {
                    b.Property<int>("vote_session_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("vote_session_id"));

                    b.Property<int>("no_count")
                        .HasColumnType("int");

                    b.Property<int>("pnm_id")
                        .HasColumnType("int");

                    b.Property<DateTime?>("session_close_dt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("session_open_dt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("voting_open_yn")
                        .HasColumnType("bit");

                    b.Property<int>("yes_count")
                        .HasColumnType("int");

                    b.HasKey("vote_session_id");

                    b.ToTable("PNMVoteSessions");
                });

            modelBuilder.Entity("GreekRecruit.Models.PNMVoteTracker", b =>
                {
                    b.Property<int>("tracker_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("tracker_id"));

                    b.Property<int>("user_id")
                        .HasColumnType("int");

                    b.Property<int>("vote_session_id")
                        .HasColumnType("int");

                    b.Property<DateTime>("vote_time")
                        .HasColumnType("datetime2");

                    b.HasKey("tracker_id");

                    b.ToTable("PNMVoteTrackers");
                });

            modelBuilder.Entity("GreekRecruit.Models.User", b =>
                {
                    b.Property<int>("user_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("user_id"));

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("full_name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("is_hashed_passowrd")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("organization_id")
                        .HasColumnType("int");

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("role")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("user_id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
