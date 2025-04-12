using AttendanceAppProject.ApiService;
using AttendanceAppProject.ApiService.Data;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

//DB Context connection
// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Add Swagger (make sure you have the Swashbuckle.AspNetCore package)
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Attendance API", Version = "v1" });
});

// Add API services from our ApiService project
builder.Services.AddApiServices(builder.Configuration);

// Add CORS to allow the desktop app to connect
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorOrigin",
        policy => policy.WithOrigins("https://localhost:7530", "http://localhost:7530")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});



builder.Services.AddScoped(sp => 
    new HttpClient
    { 
        BaseAddress = new Uri("https://localhost:7530") // Replace with your API's port
    });

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowDesktopApp", policy =>
//    {
//        policy.AllowAnyOrigin()
//              .AllowAnyMethod()
//              .AllowAnyHeader();
//    });
//});

var app = builder.Build();
// After app.Build()
app.UseCors("AllowBlazorOrigin");

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Attendance API v1"));
}

app.UseHttpsRedirection();
app.UseCors("AllowDesktopApp");
app.UseAuthorization();
app.MapControllers();

app.Run();