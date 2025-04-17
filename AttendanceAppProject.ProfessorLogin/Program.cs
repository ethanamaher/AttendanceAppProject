using System;
using System.Net.Http;
using System.Windows;
using AttendanceAppProject.ProfessorLogin;
using AttendanceAppProject.ProfessorLogin.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AttendanceAppProject.ProfessorLogin
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public static ProfessorModel CurrentProfessor { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Configure services
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Start with the login window
            var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Configure HttpClient with base address
            services.AddSingleton<HttpClient>(provider =>
            {
                var client = new HttpClient
                {
                    BaseAddress = new Uri("http://localhost:5001/")
                };
                return client;
            });

            // Register the authentication services
            services.AddSingleton<IProfessorAuthClient, ProfessorAuthClient>();
            services.AddSingleton<IProfessorAuthService, ProfessorAuthService>();

            // Register windows with HttpClient dependency injection
            services.AddTransient<LoginWindow>();
            services.AddTransient<AttendanceWindow>(provider =>
                new AttendanceWindow(provider.GetRequiredService<HttpClient>()));
        }
    }
}