// Canh Nguyen 

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;
using AttendanceAppProject.ProfessorLogin.Models;

namespace AttendanceAppProject.ProfessorLogin
{
    public partial class App : Application
    {
        public IServiceProvider? ServiceProvider { get; private set; }
        public IConfiguration? Configuration { get; private set; }

        // Static property to store logged in professor data for use across windows
        public static ProfessorModel? CurrentProfessor { get; set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory());

                // Only add appsettings.json if it exists
                string appsettingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
                if (File.Exists(appsettingsPath))
                {
                    builder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                }

                Configuration = builder.Build();

                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                ServiceProvider = serviceCollection.BuildServiceProvider();

                var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
                loginWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting application: {ex.Message}", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register windows only - we'll handle the API connection separately
            services.AddTransient<LoginWindow>();
            services.AddTransient<AttendanceWindow>();
        }
    }
}