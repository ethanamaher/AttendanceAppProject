using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using AttendanceAppProject.Dto.Models;
using AttendanceAppProject.ProfessorLogin.Services;
using System.Net.Http;
using System.Diagnostics;

namespace AttendanceAppProject.ProfessorLogin
{
    public partial class LoginWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IProfessorAuthClient _authClient;
        //Chris Palmer
        //Professor Model is from the AttendanceAppProject.ProfessorLogin.Models namespace
        //declare empty professor instance
        public ProfessorDto? professor;

        public LoginWindow(IServiceProvider serviceProvider)
        {
            Debug.WriteLine("Login init");
            InitializeComponent();
            _serviceProvider = serviceProvider;
            //will return null if authentication failed
            //will return ProfessorModel otherwise
            _authClient = serviceProvider.GetRequiredService<IProfessorAuthClient>();
            ProfessorIdTextBox.Focus();

            this.Loaded += async (s, e) => await CheckApiConnectionAsync();

        }

        private async Task CheckApiConnectionAsync()
        {
            while (true)
            {
                try
                {
                    using var client = new HttpClient { BaseAddress = new Uri("https://localhost:7530/") };
                    var response = await client.GetAsync("api/student");
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    MessageBox.Show("✅ Connected to API!", "API Status", MessageBoxButton.OK, MessageBoxImage.Information);
                    //    return;
                    //}
                }
                catch
                {
                    // Suppress errors
                }

                await Task.Delay(2000); // Try again after 2 seconds
            }
        }

        //Chris Palmer
        public ProfessorDto GetProfessor()
        {
            return this.professor;            
        }
        public async void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            //Christopher Palmer
            try
            {

                //declaring professor object
                //get entered information
                string professorId = ProfessorIdTextBox.Text.Trim();
                string password = PasswordBox.Password;
                Debug.WriteLine("profID: " + professorId + ", pass: " + password);
                

                //Ensuring both fields are filled out
                if (string.IsNullOrWhiteSpace(professorId))
                {
                    StatusTextBlock.Text = "Please enter your Professor ID";
                    ProfessorIdTextBox.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    StatusTextBlock.Text = "Please enter your password";
                    PasswordBox.Focus();
                    return;
                }
                // instantiate professor from database. returns ProfessorModel
                this.professor = await _authClient.LoginAsync(professorId, password);
                
                
                // Clear previous status
                StatusTextBlock.Text = string.Empty;

                // Show loading message
                StatusTextBlock.Text = "Authenticating...";


                // If API fails, fall back to mock data for testing purposes
                if (this.professor == null && (
                    professorId == "js123" ||
                    professorId == "jd123" ||
                    professorId == "rj123" ||
                    professorId == "test"))
                {
                    this.professor = MockDataProvider.GetMockProfessor(professorId, password);
                    StatusTextBlock.Text = "API login failed. Using mock data for demo purposes.";
                }

                if (this.professor != null)
                {
                    // Store the professor data for use in other windows
                    App.CurrentProfessor = this.professor;

                    // Handle "Remember Me" checkbox if needed
                    bool rememberMe = RememberMeCheckBox.IsChecked ?? false;
                    // if (rememberMe) { /* Save credentials logic */ }

                    // Show a welcome message
                    MessageBox.Show($"Welcome, {this.professor.FirstName} {this.professor.LastName}!\nDepartment: {this.professor.Department}",
                        "Login Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Open the attendance window
                    var attendanceWindow = _serviceProvider.GetRequiredService<AttendanceWindow>();
                    attendanceWindow.Show();

                    // Update MainWindow reference
                    Application.Current.MainWindow = attendanceWindow;

                    // Close the login window
                    this.Close();
                }
                else
                {
                    StatusTextBlock.Text = "Invalid Professor ID or password";
                    PasswordBox.Password = string.Empty;
                    PasswordBox.Focus();
                }
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = "Error during login";
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ForgotPasswordLink_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Please contact your department administrator to reset your password.",
                           "Forgot Password",
                           MessageBoxButton.OK,
                           MessageBoxImage.Information);
        }
    }
}