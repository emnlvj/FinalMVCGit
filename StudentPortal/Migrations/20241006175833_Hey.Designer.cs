﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using StudentPortal.Data;

#nullable disable

namespace StudentPortal.Migrations
{
    [DbContext(typeof(StudentEntryDbContext))]
    [Migration("20241006175833_Hey")]
    partial class Hey
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PreUnits")
                        .HasColumnType("int");

                    b.Property<string>("SubjCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

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

            modelBuilder.Entity("StudentPortal.Models.Student", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Age")
                        .HasColumnType("int");

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

                    b.Property<string>("Section")
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
                            Age = 22,
                            Course = "BSIT",
                            FName = "Victor",
                            LName = "Jacaba",
                            MName = "Edisan",
                            Section = "3F",
                            Year = 3,
                            count = 1
                        },
                        new
                        {
                            Id = 2321123,
                            Age = 20,
                            Course = "BSN",
                            FName = "Sheena",
                            LName = "Jacaba",
                            MName = "Torreta",
                            Section = "3E",
                            Year = 3,
                            count = 2
                        });
                });

            modelBuilder.Entity("StudentPortal.Models.Subject", b =>
                {
                    b.Property<string>("SubjCode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CatCourse")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CourseCode")
                        .HasColumnType("int");

                    b.Property<string>("CurrYear")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Descript")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Offering")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Units")
                        .HasColumnType("int");

                    b.HasKey("SubjCode");

                    b.ToTable("SubjectInfo");

                    b.HasData(
                        new
                        {
                            SubjCode = "IMDBSYS31",
                            CatCourse = "BSIT",
                            CourseCode = 99854,
                            CurrYear = "2024-2025",
                            Descript = "Information Management Database System",
                            Offering = "Summer",
                            Units = 3
                        },
                        new
                        {
                            SubjCode = "IT-FREKYI1",
                            CatCourse = "BSIT",
                            CourseCode = 99886,
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

            modelBuilder.Entity("StudentPortal.Models.Subject", b =>
                {
                    b.Navigation("PreRequisite");
                });
#pragma warning restore 612, 618
        }
    }
}
