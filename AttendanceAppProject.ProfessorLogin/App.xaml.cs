// Canh Nguyen 

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Windows;
using System.Net.Http;
using System.Net.Http.Headers;
using AttendanceAppProject.ProfessorLogin.Models;

namespace AttendanceAppProject.ProfessorLogin
{
    public partial class App : Application
    {

        //setup http context
        //Chris Palmer
        public static HttpClient __httpClient { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            __httpClient = new HttpClient()
            {
                BaseAddress = new Uri("http://localhost:5001/") // Trailing slash is important!
            };

            // Add default headers if needed
            __httpClient.DefaultRequestHeaders.Accept.Clear();
            __httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }


        public IServiceProvider? ServiceProvider { get; private set; }
        public IConfiguration? Configuration { get; private set; }

        // Static property to store logged in professor data for use across windows
        public static Professor CurrentProfessor { get; set; }

        private static App _current;
        public static new App Current => _current ??= (App)Application.Current;


        private void Application_Startup(object sender, StartupEventArgs e)
        {
            _current = this; // Set the static reference
            base.OnStartup(e);

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
            // Register the API services when integrating to actual database

            /*
            if (Configuration != null)
            {
                services.AddApiServices(Configuration);
            } 
            */

            // Register windows only - we'll handle the API connection separately
            services.AddTransient<LoginWindow>();
            services.AddTransient<AttendanceWindow>();
        }
    }
}