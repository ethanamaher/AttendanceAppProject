using AttendanceAppProject.ApiService.Data;
using Microsoft.EntityFrameworkCore;

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

// Enable Controllers
builder.Services.AddControllers();

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