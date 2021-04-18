﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PragyoSala.Services.Data;

namespace prayogsala_services.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210413143228_video")]
    partial class video
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.4");

            modelBuilder.Entity("PragyoSala.Services.Models.Chapter", b =>
                {
                    b.Property<int>("ChapterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ChapterName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("ChapterId");

                    b.HasIndex("CourseId");

                    b.ToTable("chapters");
                });

            modelBuilder.Entity("PragyoSala.Services.Models.Course", b =>
                {
                    b.Property<int>("CourseId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("CourseTitle")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Image")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("CourseId");

                    b.HasIndex("UserId");

                    b.ToTable("courses");
                });

            modelBuilder.Entity("PragyoSala.Services.Models.CourseRate", b =>
                {
                    b.Property<int>("CourseRateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CratedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("Rate")
                        .HasColumnType("double");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.HasKey("CourseRateId");

                    b.HasIndex("CourseId");

                    b.ToTable("course_rate");
                });

            modelBuilder.Entity("PragyoSala.Services.Models.Topic", b =>
                {
                    b.Property<int>("TopicId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("ChapterId")
                        .HasColumnType("int");

                    b.Property<int>("CourseId")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsWatched")
                        .HasColumnType("tinyint(1)");

                    b.Property<double>("Length")
                        .HasColumnType("double");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<string>("TopicName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("VideoId")
                        .HasColumnType("int");

                    b.HasKey("TopicId");

                    b.HasIndex("ChapterId");

                    b.HasIndex("CourseId");

                    b.HasIndex("VideoId");

                    b.ToTable("topics");
                });

            modelBuilder.Entity("PragyoSala.Services.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Email")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("FirstName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("LastName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Password")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("PhoneNo")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Role")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PragyoSala.Services.Models.Video", b =>
                {
                    b.Property<int>("VideoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("DeletedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("UpdatedAt")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("VideoName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("VideoId");

                    b.ToTable("videos");
                });

            modelBuilder.Entity("PragyoSala.Services.Models.Chapter", b =>
                {
                    b.HasOne("PragyoSala.Services.Models.Course", "Course")
                        .WithMany("Chapters")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("PragyoSala.Services.Models.Course", b =>
                {
                    b.HasOne("PragyoSala.Services.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("PragyoSala.Services.Models.CourseRate", b =>
                {
                    b.HasOne("PragyoSala.Services.Models.Course", "Course")
                        .WithMany("Rates")
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("PragyoSala.Services.Models.Topic", b =>
                {
                    b.HasOne("PragyoSala.Services.Models.Chapter", "Chapter")
                        .WithMany("Topics")
                        .HasForeignKey("ChapterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PragyoSala.Services.Models.Course", "Course")
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PragyoSala.Services.Models.Video", "Video")
                        .WithMany()
                        .HasForeignKey("VideoId");

                    b.Navigation("Chapter");

                    b.Navigation("Course");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("PragyoSala.Services.Models.Chapter", b =>
                {
                    b.Navigation("Topics");
                });

            modelBuilder.Entity("PragyoSala.Services.Models.Course", b =>
                {
                    b.Navigation("Chapters");

                    b.Navigation("Rates");
                });
#pragma warning restore 612, 618
        }
    }
}
