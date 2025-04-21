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
            Debug.WriteLine("On Startup");
            //__httpClient = new HttpClient()
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
        //public static Professor CurrentProfessor { get; set; }

        private static App _current;
        public static new App Current => _current ??= (App)Application.Current;


        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            Debug.WriteLine("App Startup");
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

                //display login window
                //loginWindow = new LoginWindow()
                var loginWindow = ServiceProvider.GetRequiredService<LoginWindow>();
                Diagnostics.Debug.WriteLine("login instance created");
                loginWindow.Show();
                

                //some kind of await statement to wait for login verification
                ProfessorModel currentProf = await loginWindow.GetProfessorAsync();
                
                Debug.WriteLine("LOGIN WINDOW CREATED AND AWAITED THE GET PROF");
                Debug.WriteLine(currentProf);
                loginWindow.Close();

                //decalring dashboard
                var attendanceWindow = _serviceProvider.GetRequiredService<AttendanceWindow>();
                attendanceWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error starting application: {ex.Message}", "Startup Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ConfigureServices(IServiceCollection services)
        {
            if (Configuration != null)
            {
                services.AddHttpClient("ApiClient", client =>
                {
                    client.BaseAddress = new Uri(Configuration["ApiBaseUrl"]);
                });
            }

            services.AddTransient<LoginWindow>();
            services.AddTransient<AttendanceWindow>();
        }
    }
}