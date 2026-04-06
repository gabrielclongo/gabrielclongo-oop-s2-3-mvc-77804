using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VgcCollege.Web.Models;

namespace VgcCollege.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

      
        public DbSet<StudentProfile> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseEnrollment> CourseEnrollments { get; set; }
        public DbSet<Assignment> Assignments { get; set; }
        public DbSet<AssignmentResult> AssignmentResults { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamResult> ExamResults { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            
            builder.Entity<StudentProfile>()
                .HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(s => s.IdentityUserId)
                .OnDelete(DeleteBehavior.Restrict);

            
            builder.Entity<CourseEnrollment>()
                .HasOne(e => e.StudentProfile)
                .WithMany()
                .HasForeignKey(e => e.StudentProfileId)
                .OnDelete(DeleteBehavior.Cascade);

           
            builder.Entity<CourseEnrollment>()
                .HasOne(e => e.Course)
                .WithMany()
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

           
            builder.Entity<Assignment>()
                .HasOne(a => a.Course)
                .WithMany()
                .HasForeignKey(a => a.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

           
            builder.Entity<AssignmentResult>()
                .HasOne(ar => ar.Assignment)
                .WithMany()
                .HasForeignKey(ar => ar.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

           
            builder.Entity<AssignmentResult>()
                .HasOne(ar => ar.StudentProfile)
                .WithMany()
                .HasForeignKey(ar => ar.StudentProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            
            builder.Entity<Exam>()
                .HasOne(e => e.Course)
                .WithMany()
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

          
            builder.Entity<ExamResult>()
                .HasOne(er => er.Exam)
                .WithMany()
                .HasForeignKey(er => er.ExamId)
                .OnDelete(DeleteBehavior.Cascade);

            
            builder.Entity<ExamResult>()
                .HasOne(er => er.StudentProfile)
                .WithMany()
                .HasForeignKey(er => er.StudentProfileId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}