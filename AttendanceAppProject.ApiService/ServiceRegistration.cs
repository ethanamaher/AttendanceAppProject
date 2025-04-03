using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AttendanceAppProject.ApiService.Data;

namespace AttendanceAppProject.ApiService
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Register DbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly("AttendanceAppProject.Web")));

            // Register services
            services.AddScoped<IAttendanceService, AttendanceService>();
            services.AddScoped<IProfessorAuthService, ProfessorAuthService>();

            return services;
        }
    }
}