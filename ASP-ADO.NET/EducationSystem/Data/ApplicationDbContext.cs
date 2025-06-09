using Microsoft.EntityFrameworkCore;
using EducationSystem.Models;

namespace EducationSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Course> Courses { get; set; }

        // Настройка отношений с использованием Fluent API
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка отношения многие-ко-многим между Student и Course
            modelBuilder.Entity<Student>()
                .HasMany(s => s.Courses)
                .WithMany(c => c.Students)
                .UsingEntity<Dictionary<string, object>>(
                    "StudentCourse",
                    j => j
                        .HasOne<Course>()
                        .WithMany()
                        .HasForeignKey("CourseId")
                        .OnDelete(DeleteBehavior.Cascade),
                    j => j
                        .HasOne<Student>()
                        .WithMany()
                        .HasForeignKey("StudentId")
                        .OnDelete(DeleteBehavior.Cascade));

            // Настройка отношения один-ко-многим между Teacher и Course
            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Courses)
                .WithOne(c => c.Teacher)
                .HasForeignKey(c => c.TeacherId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
