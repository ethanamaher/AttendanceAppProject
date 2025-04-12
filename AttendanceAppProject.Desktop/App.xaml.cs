// App.xaml.cs
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;
using AttendanceAppProject.ApiService;
using System.Net.Http;
using AttendanceAppProject.ApiService.Models;

namespace AttendanceAppProject.Desktop
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

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            // Register your API client with a fallback URL if configuration is missing
            services.AddHttpClient<IAttendanceApiClient, AttendanceApiClient>(client =>
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
            services.AddTransient<MainWindow>();
        }
    }

    internal class AttendanceApiClient : IAttendanceApiClient
    {
        private readonly HttpClient _httpClient;
        public AttendanceApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<List<AttendanceRecord>> GetAttendanceRecordsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SubmitAttendanceAsync(AttendanceRecord record)
        {
            throw new NotImplementedException();
        }
        // Implement the methods for IAttendanceApiClient here
    }
}