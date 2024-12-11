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

        public DbSet<StudentEnrollment> StudentEnrollment { get; set; }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
       
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Subject)
                .WithMany(sch => sch.Schedule)
                .HasForeignKey(s => s.SubjCode)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<PreRequisite>()
                .HasOne(pr => pr.Subject)
                .WithMany(s => s.PreRequisite)
                .HasForeignKey(pr => pr.SubjCode)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<StudentEnrollment>()
             .HasKey(ss => new { ss.Id, ss.SubEdpCode }); // Composite key

            modelBuilder.Entity<StudentEnrollment>()
                .HasOne(ss => ss.Student)
                .WithMany(s => s.StudentEnrollment)
                .HasForeignKey(ss => ss.Id);

            modelBuilder.Entity<StudentEnrollment>()
                .HasOne(ss => ss.Schedule)
                .WithMany(s => s.StudentEnrollment)
                .HasForeignKey(ss => ss.SubEdpCode);

            base.OnModelCreating(modelBuilder);


        }


    }
}