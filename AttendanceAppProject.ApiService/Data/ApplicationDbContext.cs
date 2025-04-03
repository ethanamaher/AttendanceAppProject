using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Models;

namespace AttendanceAppProject.ApiService.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<AttendanceRecord> AttendanceRecords { get; set; } = null!;
        public DbSet<Professor> Professors { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the AttendanceRecord entity
            modelBuilder.Entity<AttendanceRecord>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProfessorName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.StudentId).HasMaxLength(50);
                entity.Property(e => e.Class).IsRequired().HasMaxLength(100);
                entity.Property(e => e.QuizQuestion).HasMaxLength(500);
                entity.Property(e => e.QuizAnswer).HasMaxLength(500);

                // Ignore OrdinalNumber property as it's only for UI display
                entity.Ignore(e => e.OrdinalNumber);
            });

            // Configure the Professor entity
            modelBuilder.Entity<Professor>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ProfessorId).IsRequired().HasMaxLength(50);
                entity.HasIndex(e => e.ProfessorId).IsUnique();
            });

            // Seed initial professor data
            SeedData(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed initial professor data
            modelBuilder.Entity<Professor>().HasData(
                new Professor
                {
                    Id = 1,
                    ProfessorId = "js123",
                    FullName = "John Smith",
                    PasswordHash = ProfessorAuthService.CreatePasswordHash("password123"),
                    Department = "Computer Science",
                    Email = "john.smith@utdallas.edu"
                },
                new Professor
                {
                    Id = 2,
                    ProfessorId = "jd123",
                    FullName = "Jane Doe",
                    PasswordHash = ProfessorAuthService.CreatePasswordHash("password456"),
                    Department = "Mathematics",
                    Email = "jane.doe@utdallas.edu"
                },
                new Professor
                {
                    Id = 3,
                    ProfessorId = "rj123",
                    FullName = "Robert Johnson",
                    PasswordHash = ProfessorAuthService.CreatePasswordHash("password789"),
                    Department = "Physics",
                    Email = "robert.johnson@utdallas.edu"
                }
            );
        }
    }
}