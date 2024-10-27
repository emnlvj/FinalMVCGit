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
            modelBuilder.Entity<Schedule>().HasData(
                    new Schedule { 
                        EdpCode = 99876,
                        starttime = new TimeOnly(11,30),
                        endtime = new TimeOnly(12,30),
                        section = "3F",
                        roomnum = 206,
                        curryear = "2023-2024",
                        description = "Research Computer",
                        AMPM = "AM",
                        days = "MWF"
                    },

                    new Schedule
                    {
                        EdpCode = 99910,
                        starttime = new TimeOnly(1, 30),
                        endtime = new TimeOnly(2, 30),
                        section = "3F",
                        roomnum = 206,
                        curryear = "2023-2024",
                        description = "Information Management Database System",
                        AMPM = "PM",
                        days = "TTH"
                    }
                );

            modelBuilder.Entity<Subject>().HasData(
                new Subject { SubjCode = "IMDBSYS31", Descript = "Information Management Database System", Units = 3, Offering = "Summer", CatCourse = "BSIT", CourseCode = 99854, CurrYear = "2024-2025", EdpCode = 99876 },
                new Subject { SubjCode = "IT-FREKYI1", Descript = "FREE ELECTIVE KNOW YOUR INDUSTRY 1", Units = 2, Offering = "First Semester", CatCourse = "BSIT", CourseCode = 99886, CurrYear = "2024-2025", EdpCode = 99910}
            );

            // Seed PreRequisite first
            modelBuilder.Entity<PreRequisite>().HasData(
                new PreRequisite { PreSubjCode = "IMDBYS32", PreDescript = "Information Management Database System 2", PreUnits = 3, SubjCode = "IMDBSYS31" },
                new PreRequisite { PreSubjCode = "IT-FREKYI2", PreDescript = "FREE ELECTIVE KNOW YOUR INDUSTRY 2", PreUnits = 2, SubjCode = "IT-FREKYI1" }
                );

            // Seed Subject after PreRequisite
            
            // Seed Student
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 23217714, count = 1, FName = "Victor", LName = "Jacaba", MName = "Edisan", Status = "Active", Course = "BSIT", Remarks = "OLD STUDENT", Year = 3 },
                new Student { Id = 2321123, count = 2, FName = "Sheena", LName = "Jacaba", MName = "Torreta", Status = "Inactive", Course = "BSN", Remarks = "NEW STUDENT", Year = 3 }
            );

            // Configure relationships
            modelBuilder.Entity<PreRequisite>()
                .HasOne(ms => ms.Subject)
                .WithMany(ps => ps.PreRequisite)
                .HasForeignKey(ms => ms.SubjCode);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Schedule)
                .WithOne(sch => sch.Subject)
                .HasForeignKey<Subject>(s => s.EdpCode);

            base.OnModelCreating(modelBuilder);
        }

    }
}