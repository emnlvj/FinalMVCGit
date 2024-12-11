using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class TryandHopingFinal : Migration
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
                    CurrYear = table.Column<string>(type: "nvarchar(9)", maxLength: 9, nullable: false)
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
                    SubjCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreSubjectInfo", x => x.PreSubjCode);
                    table.ForeignKey(
                        name: "FK_PreSubjectInfo_SubjectInfo_SubjCode",
                        column: x => x.SubjCode,
                        principalTable: "SubjectInfo",
                        principalColumn: "SubjCode",
                        onDelete: ReferentialAction.SetNull);
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
                    status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SubjCode = table.Column<string>(type: "nvarchar(20)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleInfo", x => x.SubEdpCode);
                    table.ForeignKey(
                        name: "FK_ScheduleInfo_SubjectInfo_SubjCode",
                        column: x => x.SubjCode,
                        principalTable: "SubjectInfo",
                        principalColumn: "SubjCode",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "StudentEnrollment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SubEdpCode = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentEnrollment", x => new { x.Id, x.SubEdpCode });
                    table.ForeignKey(
                        name: "FK_StudentEnrollment_ScheduleInfo_SubEdpCode",
                        column: x => x.SubEdpCode,
                        principalTable: "ScheduleInfo",
                        principalColumn: "SubEdpCode",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentEnrollment_StudentInfo_Id",
                        column: x => x.Id,
                        principalTable: "StudentInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreSubjectInfo_SubjCode",
                table: "PreSubjectInfo",
                column: "SubjCode");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleInfo_SubjCode",
                table: "ScheduleInfo",
                column: "SubjCode");

            migrationBuilder.CreateIndex(
                name: "IX_StudentEnrollment_SubEdpCode",
                table: "StudentEnrollment",
                column: "SubEdpCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreSubjectInfo");

            migrationBuilder.DropTable(
                name: "StudentEnrollment");

            migrationBuilder.DropTable(
                name: "ScheduleInfo");

            migrationBuilder.DropTable(
                name: "StudentInfo");

            migrationBuilder.DropTable(
                name: "SubjectInfo");
        }
    }
}
