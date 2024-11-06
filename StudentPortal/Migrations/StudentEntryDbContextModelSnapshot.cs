﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentPortal.Data;

#nullable disable

namespace StudentPortal.Migrations
{
    [DbContext(typeof(StudentEntryDbContext))]
    partial class StudentEntryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("StudentPortal.Models.PreRequisite", b =>
                {
                    b.Property<string>("PreSubjCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PreDescript")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("PreUnits")
                        .HasColumnType("int");

                    b.Property<string>("SubjCode")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.HasKey("PreSubjCode");

                    b.HasIndex("SubjCode");

                    b.ToTable("PreSubjectInfo");

                    b.HasData(
                        new
                        {
                            PreSubjCode = "IMDBYS32",
                            PreDescript = "Information Management Database System 2",
                            PreUnits = 3,
                            SubjCode = "IMDBSYS31"
                        },
                        new
                        {
                            PreSubjCode = "IT-FREKYI2",
                            PreDescript = "FREE ELECTIVE KNOW YOUR INDUSTRY 2",
                            PreUnits = 2,
                            SubjCode = "IT-FREKYI1"
                        });
                });

            modelBuilder.Entity("StudentPortal.Models.Schedule", b =>
                {
                    b.Property<int>("SubEdpCode")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("SubEdpCode"));

                    b.Property<string>("AMPM")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubjCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("curryear")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("days")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeOnly>("endtime")
                        .HasColumnType("time");

                    b.Property<int>("roomnum")
                        .HasColumnType("int");

                    b.Property<string>("section")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<TimeOnly>("starttime")
                        .HasColumnType("time");

                    b.HasKey("SubEdpCode");

                    b.HasIndex("SubjCode");

                    b.ToTable("ScheduleInfo");

                    b.HasData(
                        new
                        {
                            SubEdpCode = 99876,
                            AMPM = "AM",
                            SubjCode = "IMDBSYS31",
                            curryear = "2023-2024",
                            days = "MWF",
                            description = "Information Management Database System",
                            endtime = new TimeOnly(12, 30, 0),
                            roomnum = 206,
                            section = "3F",
                            starttime = new TimeOnly(11, 30, 0)
                        },
                        new
                        {
                            SubEdpCode = 99910,
                            AMPM = "PM",
                            SubjCode = "IT-FREKYI1",
                            curryear = "2023-2024",
                            days = "TTH",
                            description = "FREE ELECTIVE KNOW YOUR INDUSTRY",
                            endtime = new TimeOnly(2, 30, 0),
                            roomnum = 206,
                            section = "3F",
                            starttime = new TimeOnly(1, 30, 0)
                        });
                });

            modelBuilder.Entity("StudentPortal.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Course")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Remarks")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.Property<int>("count")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("StudentInfo");

                    b.HasData(
                        new
                        {
                            Id = 23217714,
                            Course = "BSIT",
                            FName = "Victor",
                            LName = "Jacaba",
                            MName = "Edisan",
                            Remarks = "OLD STUDENT",
                            Status = "Active",
                            Year = 3,
                            count = 1
                        },
                        new
                        {
                            Id = 2321123,
                            Course = "BSN",
                            FName = "Sheena",
                            LName = "Jacaba",
                            MName = "Torreta",
                            Remarks = "NEW STUDENT",
                            Status = "Inactive",
                            Year = 3,
                            count = 2
                        });
                });

            modelBuilder.Entity("StudentPortal.Models.Subject", b =>
                {
                    b.Property<string>("SubjCode")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("CatCourse")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("CurrYear")
                        .IsRequired()
                        .HasMaxLength(9)
                        .HasColumnType("nvarchar(9)");

                    b.Property<string>("Descript")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Offering")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("Units")
                        .HasColumnType("int");

                    b.HasKey("SubjCode");

                    b.ToTable("SubjectInfo");

                    b.HasData(
                        new
                        {
                            SubjCode = "IMDBSYS31",
                            CatCourse = "BSIT",
                            CurrYear = "2024-2025",
                            Descript = "Information Management Database System",
                            Offering = "Summer",
                            Units = 3
                        },
                        new
                        {
                            SubjCode = "IT-FREKYI1",
                            CatCourse = "BSIT",
                            CurrYear = "2024-2025",
                            Descript = "FREE ELECTIVE KNOW YOUR INDUSTRY 1",
                            Offering = "First Semester",
                            Units = 2
                        });
                });

            modelBuilder.Entity("StudentPortal.Models.PreRequisite", b =>
                {
                    b.HasOne("StudentPortal.Models.Subject", "Subject")
                        .WithMany("PreRequisite")
                        .HasForeignKey("SubjCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("StudentPortal.Models.Schedule", b =>
                {
                    b.HasOne("StudentPortal.Models.Subject", "Subject")
                        .WithMany("Schedule")
                        .HasForeignKey("SubjCode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subject");
                });

            modelBuilder.Entity("StudentPortal.Models.Subject", b =>
                {
                    b.Navigation("PreRequisite");

                    b.Navigation("Schedule");
                });
#pragma warning restore 612, 618
        }
    }
}
