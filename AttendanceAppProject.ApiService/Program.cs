using AttendanceAppProject.ApiService.Data.Models;
using Microsoft.EntityFrameworkCore;
using AttendanceAppProject.ApiService.Data;
using Microsoft.AspNetCore.Builder;
using AttendanceAppProject.ApiService.Dto.Models;
using AttendanceAppProject.ApiService.JsonConverters;
using AttendanceAppProject.ApiService.Services;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables 
builder.Configuration.AddEnvironmentVariables();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
	?? throw new InvalidOperationException("DefaultConnection is missing in appsettings.json");

// Add service defaults & Aspire client integrations.
// builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();

// Register EF Core with MySQL, add database context
builder.Services.AddDbContext<ApplicationDbContext>(options =>
options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
		ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Enable Controllers, add JSON converters
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
    options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
});

// Register Service Classes
builder.Services.AddScoped<AttendanceInstanceService>();
builder.Services.AddScoped<ClassScheduleService>();
builder.Services.AddScoped<ClassService>();
builder.Services.AddScoped<PasswordService>();
builder.Services.AddScoped<ProfessorService>();
builder.Services.AddScoped<QuizAnswerService>();
builder.Services.AddScoped<QuizInstanceService>();
builder.Services.AddScoped<QuizQuestionService>();
builder.Services.AddScoped<StudentClassService>();
builder.Services.AddScoped<StudentService>();


// Add CORS Policy 
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowAll",
		policy => policy.AllowAnyOrigin()
						.AllowAnyMethod()
						.AllowAnyHeader());
});

var app = builder.Build();

// Enable CORS
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

// app.MapDefaultEndpoints();
app.MapControllers();

app.Run();