using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class TryandCatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    count = table.Column<int>(type: "int", nullable: false),
                    FName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Course = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectInfo",
                columns: table => new
                {
                    SubjCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Descript = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Units = table.Column<int>(type: "int", nullable: false),
                    Offering = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CatCourse = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CurrYear = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false),
                    EdpCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectInfo", x => x.SubjCode);
                });

            migrationBuilder.CreateTable(
                name: "PreSubjectInfo",
                columns: table => new
                {
                    PreSubjCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PreDescript = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PreUnits = table.Column<int>(type: "int", nullable: false),
                    SubjCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreSubjectInfo", x => x.PreSubjCode);
                    table.ForeignKey(
                        name: "FK_PreSubjectInfo_SubjectInfo_SubjCode",
                        column: x => x.SubjCode,
                        principalTable: "SubjectInfo",
                        principalColumn: "SubjCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScheduleInfo",
                columns: table => new
                {
                    SubEdpCode = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    starttime = table.Column<TimeOnly>(type: "time", nullable: false),
                    endtime = table.Column<TimeOnly>(type: "time", nullable: false),
                    section = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    roomnum = table.Column<int>(type: "int", nullable: false),
                    curryear = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AMPM = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    days = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjCode = table.Column<string>(type: "nvarchar(20)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleInfo", x => x.SubEdpCode);
                    table.ForeignKey(
                        name: "FK_ScheduleInfo_SubjectInfo_SubjCode",
                        column: x => x.SubjCode,
                        principalTable: "SubjectInfo",
                        principalColumn: "SubjCode",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "StudentInfo",
                columns: new[] { "Id", "Course", "FName", "LName", "MName", "Remarks", "Status", "Year", "count" },
                values: new object[,]
                {
                    { 2321123, "BSN", "Sheena", "Jacaba", "Torreta", "NEW STUDENT", "Inactive", 3, 2 },
                    { 23217714, "BSIT", "Victor", "Jacaba", "Edisan", "OLD STUDENT", "Active", 3, 1 }
                });

            migrationBuilder.InsertData(
                table: "SubjectInfo",
                columns: new[] { "SubjCode", "CatCourse", "CurrYear", "Descript", "EdpCode", "Offering", "Units" },
                values: new object[,]
                {
                    { "IMDBSYS31", "BSIT", "2024-2025", "Information Management Database System", 99876, "Summer", 3 },
                    { "IT-FREKYI1", "BSIT", "2024-2025", "FREE ELECTIVE KNOW YOUR INDUSTRY 1", 99910, "First Semester", 2 }
                });

            migrationBuilder.InsertData(
                table: "PreSubjectInfo",
                columns: new[] { "PreSubjCode", "PreDescript", "PreUnits", "SubjCode" },
                values: new object[,]
                {
                    { "IMDBYS32", "Information Management Database System 2", 3, "IMDBSYS31" },
                    { "IT-FREKYI2", "FREE ELECTIVE KNOW YOUR INDUSTRY 2", 2, "IT-FREKYI1" }
                });

            migrationBuilder.InsertData(
                table: "ScheduleInfo",
                columns: new[] { "SubEdpCode", "AMPM", "SubjCode", "curryear", "days", "description", "endtime", "roomnum", "section", "starttime" },
                values: new object[,]
                {
                    { 99876, "AM", "IMDBSYS31", "2023-2024", "MWF", "Information Management Database System", new TimeOnly(12, 30, 0), 206, "3F", new TimeOnly(11, 30, 0) },
                    { 99910, "PM", "IT-FREKYI1", "2023-2024", "TTH", "FREE ELECTIVE KNOW YOUR INDUSTRY", new TimeOnly(2, 30, 0), 206, "3F", new TimeOnly(1, 30, 0) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreSubjectInfo_SubjCode",
                table: "PreSubjectInfo",
                column: "SubjCode");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleInfo_SubjCode",
                table: "ScheduleInfo",
                column: "SubjCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreSubjectInfo");

            migrationBuilder.DropTable(
                name: "ScheduleInfo");

            migrationBuilder.DropTable(
                name: "StudentInfo");

            migrationBuilder.DropTable(
                name: "SubjectInfo");
        }
    }
}
