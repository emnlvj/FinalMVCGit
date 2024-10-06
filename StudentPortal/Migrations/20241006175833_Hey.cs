using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StudentPortal.Migrations
{
    /// <inheritdoc />
    public partial class Hey : Migration
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
                    Age = table.Column<int>(type: "int", nullable: false),
                    Course = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Section = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    SubjCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Descript = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Units = table.Column<int>(type: "int", nullable: false),
                    Offering = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CatCourse = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CourseCode = table.Column<int>(type: "int", nullable: false),
                    CurrYear = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    PreDescript = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PreUnits = table.Column<int>(type: "int", nullable: false),
                    SubjCode = table.Column<string>(type: "nvarchar(450)", nullable: false)
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

            migrationBuilder.InsertData(
                table: "StudentInfo",
                columns: new[] { "Id", "Age", "Course", "FName", "LName", "MName", "Section", "Year", "count" },
                values: new object[,]
                {
                    { 2321123, 20, "BSN", "Sheena", "Jacaba", "Torreta", "3E", 3, 2 },
                    { 23217714, 22, "BSIT", "Victor", "Jacaba", "Edisan", "3F", 3, 1 }
                });

            migrationBuilder.InsertData(
                table: "SubjectInfo",
                columns: new[] { "SubjCode", "CatCourse", "CourseCode", "CurrYear", "Descript", "Offering", "Units" },
                values: new object[,]
                {
                    { "IMDBSYS31", "BSIT", 99854, "2024-2025", "Information Management Database System", "Summer", 3 },
                    { "IT-FREKYI1", "BSIT", 99886, "2024-2025", "FREE ELECTIVE KNOW YOUR INDUSTRY 1", "First Semester", 2 }
                });

            migrationBuilder.InsertData(
                table: "PreSubjectInfo",
                columns: new[] { "PreSubjCode", "PreDescript", "PreUnits", "SubjCode" },
                values: new object[,]
                {
                    { "IMDBYS32", "Information Management Database System 2", 3, "IMDBSYS31" },
                    { "IT-FREKYI2", "FREE ELECTIVE KNOW YOUR INDUSTRY 2", 2, "IT-FREKYI1" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_PreSubjectInfo_SubjCode",
                table: "PreSubjectInfo",
                column: "SubjCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PreSubjectInfo");

            migrationBuilder.DropTable(
                name: "StudentInfo");

            migrationBuilder.DropTable(
                name: "SubjectInfo");
        }
    }
}
