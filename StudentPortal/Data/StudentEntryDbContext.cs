using Microsoft.EntityFrameworkCore;
using StudentPortal.Models;

namespace StudentPortal.Data
{
    public class StudentEntryDbContext : DbContext
    {
        public StudentEntryDbContext(DbContextOptions<StudentEntryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> StudentInfo { get; set; }
        public DbSet<Subject> SubjectInfo { get; set; }
        public DbSet<PreRequisite> PreSubjectInfo { get; set; }
        public DbSet<Schedule> ScheduleInfo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships first
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Subject)
                .WithOne(sch => sch.Schedule)
                .HasForeignKey<Schedule>(s => s.SubjCode);

            modelBuilder.Entity<PreRequisite>()
                .HasOne(pr => pr.Subject)
                .WithMany(s => s.PreRequisite)
                .HasForeignKey(pr => pr.SubjCode);

            // Seed Subject first
            modelBuilder.Entity<Subject>().HasData(
                new Subject
                {
                    SubjCode = "IMDBSYS31",
                    Descript = "Information Management Database System",
                    Units = 3,
                    Offering = "Summer",
                    CatCourse = "BSIT",
                    EdpCode = 99876, // Note: Must match EDPCode in future Schedule
                    PreCode = "IMDBSYS3",
                    CurrYear = "2024-2025"
                },
                new Subject
                {
                    SubjCode = "IT-FREKYI1",
                    Descript = "FREE ELECTIVE KNOW YOUR INDUSTRY 1",
                    Units = 2,
                    Offering = "First Semester",
                    CatCourse = "BSIT",
                    EdpCode = 99910, // Note: Must match EDPCode in future Schedule
                    PreCode = "IT_FREKYI3",
                    CurrYear = "2024-2025"
                }
            );

            // Seed Schedule next, now that Subjects are already in the database
            modelBuilder.Entity<Schedule>().HasData(
                new Schedule
                {
                    SubEdpCode = 99876,
                    starttime = new TimeOnly(11, 30),
                    endtime = new TimeOnly(12, 30),
                    section = "3F",
                    roomnum = 206,
                    curryear = "2023-2024",
                    description = "Information Management Database System",
                    AMPM = "AM",
                    days = "MWF",
                    SubjCode = "IMDBSYS31"
                },
                new Schedule
                {
                    SubEdpCode = 99910,
                    starttime = new TimeOnly(1, 30),
                    endtime = new TimeOnly(2, 30),
                    section = "3F",
                    roomnum = 206,
                    curryear = "2023-2024",
                    description = "FREE ELECTIVE KNOW YOUR INDUSTRY",
                    AMPM = "PM",
                    days = "TTH",
                    SubjCode = "IT-FREKYI1"
                }
            );

            // Seed PreRequisite after Subject
            modelBuilder.Entity<PreRequisite>().HasData(
                new PreRequisite
                {
                    PreSubjCode = "IMDBYS32",
                    PreDescript = "Information Management Database System 2",
                    PreUnits = 3,
                    SubjCode = "IMDBSYS31" // References an existing SubjCode in Subject
                },
                new PreRequisite
                {
                    PreSubjCode = "IT-FREKYI2",
                    PreDescript = "FREE ELECTIVE KNOW YOUR INDUSTRY 2",
                    PreUnits = 2,
                    SubjCode = "IT-FREKYI1" // References an existing SubjCode in Subject
                }
            );

            // Seed Student independently
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 23217714, count = 1, FName = "Victor", LName = "Jacaba", MName = "Edisan", Status = "Active", Course = "BSIT", Remarks = "OLD STUDENT", Year = 3 },
                new Student { Id = 2321123, count = 2, FName = "Sheena", LName = "Jacaba", MName = "Torreta", Status = "Inactive", Course = "BSN", Remarks = "NEW STUDENT", Year = 3 }
            );

            base.OnModelCreating(modelBuilder);
        }


    }
}