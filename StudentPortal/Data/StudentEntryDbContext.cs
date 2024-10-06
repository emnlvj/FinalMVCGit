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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>().HasData(
                new Subject { SubjCode = "IMDBSYS31", Descript = "Information Management Database System", Units = 3, Offering = "Summer", CatCourse = "BSIT", CourseCode = 99854, CurrYear = "2024-2025" },
                new Subject { SubjCode = "IT-FREKYI1", Descript = "FREE ELECTIVE KNOW YOUR INDUSTRY 1", Units = 2, Offering = "First Semester", CatCourse = "BSIT", CourseCode = 99886, CurrYear = "2024-2025" }
            );

            // Seed PreRequisite first
            modelBuilder.Entity<PreRequisite>().HasData(
                new PreRequisite { PreSubjCode = "IMDBYS32", PreDescript = "Information Management Database System 2", PreUnits = 3, SubjCode = "IMDBSYS31" },
                new PreRequisite { PreSubjCode = "IT-FREKYI2", PreDescript = "FREE ELECTIVE KNOW YOUR INDUSTRY 2", PreUnits = 2, SubjCode = "IT-FREKYI1" }
                );

            // Seed Subject after PreRequisite
            
            // Seed Student
            modelBuilder.Entity<Student>().HasData(
                new Student { Id = 23217714, count = 1, FName = "Victor", LName = "Jacaba", MName = "Edisan", Age = 22, Course = "BSIT", Section = "3F", Year = 3 },
                new Student { Id = 2321123, count = 2, FName = "Sheena", LName = "Jacaba", MName = "Torreta", Age = 20, Course = "BSN", Section = "3E", Year = 3 }
            );

            // Configure relationships
            modelBuilder.Entity<PreRequisite>()
                .HasOne(ms => ms.Subject)
                .WithMany(ps => ps.PreRequisite)
                .HasForeignKey(ms => ms.SubjCode);

            base.OnModelCreating(modelBuilder);
        }

    }
}