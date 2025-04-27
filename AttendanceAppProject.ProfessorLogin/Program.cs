using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Windows;
using AttendanceAppProject.ProfessorLogin.Services;
using AttendanceAppProject.Dto.Models;
using System.Diagnostics;

namespace AttendanceAppProject.ProfessorLogin
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public static ProfessorDto CurrentProfessor { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Debug.WriteLine("Program.cs & Application Startup");
            var services = new ServiceCollection();
            Debug.WriteLine("Services Configuration");
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();

            var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
            Debug.WriteLine("login instance created");
            loginWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new HttpClient
            {
                BaseAddress = new Uri("https://localhost:7530/")
            });

            services.AddSingleton<IProfessorAuthClient, ProfessorAuthClient>();
            services.AddTransient<LoginWindow>();
            services.AddTransient<ClassCreationWindow>();
            services.AddTransient<ClassDashboardWindow>();
            services.AddTransient<AttendanceWindow>();
        }
    }
}