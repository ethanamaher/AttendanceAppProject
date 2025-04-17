using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace AttendanceAppProject.ApiService.Data;

public partial class ApplicationDbContext : DbContext
{
	public ApplicationDbContext()
	{
	}

	public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		: base(options)
	{
	}

	public virtual DbSet<AttendanceInstance> AttendanceInstances { get; set; }

	public virtual DbSet<Class> Classes { get; set; }

	public virtual DbSet<ClassSchedule> ClassSchedules { get; set; }

	public virtual DbSet<Password> Passwords { get; set; }

	public virtual DbSet<Professor> Professors { get; set; }

	public virtual DbSet<QuizInstance> QuizInstances { get; set; }

	public virtual DbSet<QuizQuestion> QuizQuestions { get; set; }
    public virtual DbSet<QuizAnswer> QuizAnswers { get; set; }

    public virtual DbSet<QuizResponse> QuizResponses { get; set; }

	public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<StudentClass> StudentClasses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

			string connectionString = configuration.GetConnectionString("DefaultConnection");
			optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
		}
	}


	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		modelBuilder
			.UseCollation("utf8mb4_0900_ai_ci")
			.HasCharSet("utf8mb4");

		modelBuilder.Entity<AttendanceInstance>(entity =>
		{
			entity.HasKey(e => e.AttendanceId).HasName("PRIMARY");

			entity.ToTable("attendance_instance");

			entity.HasIndex(e => e.ClassId, "fk_attendanceInstance_classId");

			entity.HasIndex(e => e.StudentId, "fk_attendanceInstance_studentId");

			entity.Property(e => e.AttendanceId).HasColumnName("Attendance_id");
			entity.Property(e => e.ClassId).HasColumnName("Class_id");
			entity.Property(e => e.DateTime)
				.HasColumnType("timestamp")
				.HasColumnName("Date_time");
			entity.Property(e => e.ExcusedAbsence).HasColumnName("Excused_absence");
			entity.Property(e => e.IpAddress)
				.HasMaxLength(45)
				.HasColumnName("Ip_address");
			entity.Property(e => e.IsLate).HasColumnName("Is_late");
			entity.Property(e => e.StudentId)
				.HasMaxLength(10)
				.IsFixedLength()
				.HasColumnName("Student_id");

			entity.HasOne(d => d.Class).WithMany(p => p.AttendanceInstances)
				.HasForeignKey(d => d.ClassId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_attendanceInstance_classId");

			entity.HasOne(d => d.Student).WithMany(p => p.AttendanceInstances)
				.HasForeignKey(d => d.StudentId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_attendanceInstance_studentId");
		});

		modelBuilder.Entity<Class>(entity =>
		{
			entity.HasKey(e => e.ClassId).HasName("PRIMARY");

			entity.ToTable("class");

			entity.HasIndex(e => e.ProfUtdId, "fk_class_profId");

			entity.Property(e => e.ClassId).HasColumnName("Class_id");
			entity.Property(e => e.ClassName)
				.HasMaxLength(100)
				.HasColumnName("Class_name");
			entity.Property(e => e.ClassNumber)
				.HasMaxLength(8)
				.IsFixedLength()
				.HasColumnName("Class_number");
			entity.Property(e => e.ClassPrefix)
				.HasMaxLength(4)
				.HasColumnName("Class_prefix");
			entity.Property(e => e.EndDate).HasColumnName("End_date");
			entity.Property(e => e.EndTime)
				.HasColumnType("time")
				.HasColumnName("End_time");
			entity.Property(e => e.ProfUtdId)
				.HasMaxLength(10)
				.IsFixedLength()
				.HasColumnName("Prof_utd_id");
			entity.Property(e => e.StartDate).HasColumnName("Start_date");
			entity.Property(e => e.StartTime)
				.HasColumnType("time")
				.HasColumnName("Start_time");

			entity.HasOne(d => d.ProfUtd).WithMany(p => p.Classes)
				.HasForeignKey(d => d.ProfUtdId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_class_profId");
		});

		modelBuilder.Entity<ClassSchedule>(entity =>
		{
			entity.HasKey(e => e.ClassScheduleId).HasName("PRIMARY");

			entity.ToTable("class_schedule");

			entity.HasIndex(e => e.ClassId, "fk_classSchedule_classId");

			entity.Property(e => e.ClassScheduleId).HasColumnName("Class_schedule_id");
			entity.Property(e => e.ClassId).HasColumnName("Class_id");
			entity.Property(e => e.DayOfWeek)
				.HasColumnType("enum('Monday','Tuesday','Wednesday','Thursday','Friday','Saturday','Sunday')")
				.HasColumnName("Day_of_Week");

			entity.HasOne(d => d.Class).WithMany(p => p.ClassSchedules)
				.HasForeignKey(d => d.ClassId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_classSchedule_classId");
		});

		modelBuilder.Entity<Password>(entity =>
		{
			entity.HasKey(e => e.PasswordId).HasName("PRIMARY");

			entity.ToTable("passwords");

			entity.HasIndex(e => e.ClassId, "fk_passwords_classId");

			entity.Property(e => e.PasswordId).HasColumnName("Password_id");
			entity.Property(e => e.ClassId).HasColumnName("Class_id");
			entity.Property(e => e.DateAssigned).HasColumnName("Date_assigned");
			entity.Property(e => e.PasswordText)
				.HasMaxLength(100)
				.HasColumnName("Password_text");

			entity.HasOne(d => d.Class).WithMany(p => p.Passwords)
				.HasForeignKey(d => d.ClassId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_passwords_classId");
		});

		modelBuilder.Entity<Professor>(entity =>
		{
			entity.HasKey(e => e.UtdId).HasName("PRIMARY");

			entity.ToTable("professor");

			entity.Property(e => e.UtdId)
				.HasMaxLength(10)
				.IsFixedLength()
				.HasColumnName("Utd_id");
			entity.Property(e => e.FirstName)
				.HasMaxLength(50)
				.HasColumnName("First_name");
			entity.Property(e => e.LastName)
				.HasMaxLength(50)
				.HasColumnName("Last_name");
		});

		modelBuilder.Entity<QuizInstance>(entity =>
		{
			entity.HasKey(e => e.QuizId).HasName("PRIMARY");

			entity.ToTable("quiz_instance");

			entity.HasIndex(e => e.ClassId, "fk_quizInstance_classId");

			entity.Property(e => e.QuizId).HasColumnName("Quiz_id");
			entity.Property(e => e.ClassId).HasColumnName("Class_id");
			entity.Property(e => e.EndTime)
				.HasColumnType("timestamp")
				.HasColumnName("End_time");
			entity.Property(e => e.StartTime)
				.HasColumnType("timestamp")
				.HasColumnName("Start_time");

			entity.HasOne(d => d.Class).WithMany(p => p.QuizInstances)
				.HasForeignKey(d => d.ClassId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_quizInstance_classId");
		});

		modelBuilder.Entity<QuizQuestion>(entity =>
		{
			entity.HasKey(e => e.QuestionId).HasName("PRIMARY");


			entity.ToTable("quiz_questions");

			entity.HasIndex(e => e.QuizId, "fk_quizQuestions_quizId");

			entity.Property(e => e.QuestionId).HasColumnName("Question_id");

			entity.Property(e => e.QuestionText)
				.HasMaxLength(500)
				.HasColumnName("Question_text");
			entity.Property(e => e.QuizId).HasColumnName("Quiz_id");

			entity.HasOne(d => d.Quiz).WithMany(p => p.QuizQuestions)
				.HasForeignKey(d => d.QuizId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_quizQuestions_quizId");
		});

        modelBuilder.Entity<QuizAnswer>(entity =>
        {
            entity.HasKey(e => e.AnswerId).HasName("PRIMARY");
            entity.Property(e => e.AnswerId).HasColumnName("Answer_id");

            entity.ToTable("quiz_answers");

            entity.HasIndex(e => e.QuestionId, "fk_questionId_quizQuestions");
            entity.HasIndex(e => e.QuizId, "fk_quizId_quizInstance");

            entity.Property(e => e.QuizId).HasColumnName("Quiz_id");
            entity.Property(e => e.QuestionId).HasColumnName("Question_id");

            entity.Property(e => e.AnswerText)
                .HasMaxLength(255)
                .HasColumnName("Answer_text");

            entity.Property(e => e.IsCorrect).HasColumnName("Answer_isCorrect");
        });

        modelBuilder.Entity<QuizResponse>(entity =>
		{
			entity.HasKey(e => e.ResponseId).HasName("PRIMARY");

			entity.ToTable("quiz_responses");

			entity.HasIndex(e => e.QuizInstanceId, "fk_quizResponses_quizInstanceid");

			entity.HasIndex(e => e.QuizQuestionId, "fk_quizResponses_quizQuestionId");

			entity.HasIndex(e => e.StudentId, "fk_quizResponses_studentId");

			entity.Property(e => e.ResponseId).HasColumnName("Response_id");
			entity.Property(e => e.QuizInstanceId).HasColumnName("Quiz_instance_id");
			entity.Property(e => e.QuizQuestionId).HasColumnName("Quiz_question_id");
			entity.Property(e => e.StudentId)
				.HasMaxLength(10)
				.IsFixedLength()
				.HasColumnName("Student_id");

			entity.HasOne(d => d.QuizInstance).WithMany(p => p.QuizResponses)
				.HasForeignKey(d => d.QuizInstanceId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_quizResponses_quizInstanceid");

			entity.HasOne(d => d.QuizQuestion).WithMany(p => p.QuizResponses)
				.HasForeignKey(d => d.QuizQuestionId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_quizResponses_quizQuestionId");

			entity.HasOne(d => d.Student).WithMany(p => p.QuizResponses)
				.HasForeignKey(d => d.StudentId)
				.OnDelete(DeleteBehavior.ClientSetNull)
				.HasConstraintName("fk_quizResponses_studentId");
		});

		modelBuilder.Entity<Student>(entity =>
		{
			entity.HasKey(e => e.UtdId).HasName("PRIMARY");

			entity.ToTable("student");

			entity.HasIndex(e => e.Username, "Username").IsUnique();

			entity.Property(e => e.UtdId)
				.HasMaxLength(10)
				.IsFixedLength()
				.HasColumnName("Utd_id");
			entity.Property(e => e.FirstName)
				.HasMaxLength(50)
				.HasColumnName("First_name");
			entity.Property(e => e.LastName)
				.HasMaxLength(50)
				.HasColumnName("Last_name");
			entity.Property(e => e.Username)
				.HasMaxLength(9)
				.IsFixedLength();
		});

        modelBuilder.Entity<StudentClass>(entity =>
        {
            entity.HasKey(e => e.StudentClassId).HasName("PRIMARY");

            entity.ToTable("student_class");

            entity.Property(e => e.StudentClassId).HasColumnName("Student_class_id");

            entity.Property(e => e.StudentId)
                .HasMaxLength(10)
                .IsFixedLength()
                .HasColumnName("Student_id"); // Column name in DB

            entity.Property(e => e.ClassId)
                .HasColumnName("Class_id");

            entity.HasOne(d => d.Student)
                .WithMany(p => p.StudentClasses)
                .HasForeignKey(d => d.StudentId) 
                .HasPrincipalKey(s => s.UtdId)   // Explicitly map StudentClass.StudentId to Student.UtdId
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_studentclass_student");

            entity.HasOne(d => d.Class)
                .WithMany(p => p.StudentClasses)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_studentclass_class");
        });

        OnModelCreatingPartial(modelBuilder);
    }

	partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
