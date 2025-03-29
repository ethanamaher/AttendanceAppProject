using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
// using AttendanceAppProject.ApiService;

namespace AttendanceAppProject.ProfessorLogin
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
            loginWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register your API client
            services.AddHttpClient<IProfessorAuthClient, ProfessorAuthClient>(client =>
            {
                string baseUrl = Configuration["ApiSettings:BaseUrl"];
                if (string.IsNullOrEmpty(baseUrl))
                {
                    // Default fallback URL
                    baseUrl = "https://localhost:5001";
                    Console.WriteLine("Warning: ApiSettings:BaseUrl not found in configuration, using default URL");
                }
                client.BaseAddress = new Uri(baseUrl);
            });

            // Register main window
            services.AddTransient<LoginWindow>();
        }

        // Method to launch the attendance app
        public static void LaunchAttendanceApp()
        {
            try
            {
                string appPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AttendanceAppProject.Desktop.exe");

                if (!File.Exists(appPath))
                {
                    // Try to find the app in a different location
                    string alternativePath = Path.Combine(
                        Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName,
                        "AttendanceAppProject.Desktop",
                        "bin",
                        "Debug",
                        "net8.0-windows",
                        "AttendanceAppProject.Desktop.exe");

                    if (File.Exists(alternativePath))
                    {
                        appPath = alternativePath;
                    }
                    else
                    {
                        MessageBox.Show("Could not find the Attendance Desktop application. Please make sure it is properly built.",
                            "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = appPath,
                    UseShellExecute = true
                });

                // Close the login application
                Current.Shutdown();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error launching Attendance Desktop application: {ex.Message}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}