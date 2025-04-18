using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AttendanceAppProject.ApiService.Data;
using AttendanceAppProject.ApiService.Services.DesktopApp;

namespace AttendanceAppProject.ApiService
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext - changed from UseSqlServer to UseMySql since you're using MySQL
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(
                    configuration.GetConnectionString("DefaultConnection"),
                    ServerVersion.AutoDetect(configuration.GetConnectionString("DefaultConnection"))));

            // Register services
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IProfessorAuthService, ProfessorAuthService>();
            return services;
        }
    }
}