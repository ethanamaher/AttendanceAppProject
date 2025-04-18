using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using AttendanceAppProject.Dto.Models;
using AttendanceAppProject.ProfessorLogin.Services;
using System.Net.Http;

namespace AttendanceAppProject.ProfessorLogin
{
    public partial class LoginWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IProfessorAuthClient _authClient;

        public LoginWindow(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _authClient = serviceProvider.GetRequiredService<IProfessorAuthClient>();
            ProfessorIdTextBox.Focus();

            this.Loaded += async (s, e) => await CheckApiConnectionAsync();

        }

        private async Task CheckApiConnectionAsync()
        {
            try
            {
                using var client = new HttpClient();
                client.BaseAddress = new Uri("https://localhost:7530/"); // Your actual API base URL

                var response = await client.GetAsync("api/student"); // A known working endpoint
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("✅ Successfully connected to API!", "Connection Test", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"❌ API responded with status code: {response.StatusCode}", "Connection Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"❌ Could not connect to API: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Clear previous status
                StatusTextBlock.Text = string.Empty;

                // Get entered credentials
                string professorId = ProfessorIdTextBox.Text.Trim();
                string password = PasswordBox.Password;

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

                // Show loading message
                StatusTextBlock.Text = "Authenticating...";

                // First try with the API
                var professor = await _authClient.LoginAsync(professorId, password);

                // If API fails, fall back to mock data for testing purposes
                if (professor == null && (
                    professorId == "js123" ||
                    professorId == "jd123" ||
                    professorId == "rj123" ||
                    professorId == "test"))
                {
                    professor = MockDataProvider.GetMockProfessor(professorId, password);
                    StatusTextBlock.Text = "API login failed. Using mock data for demo purposes.";
                }

                if (professor != null)
                {
                    // Store the professor data for use in other windows
                    App.CurrentProfessor = professor;

                    // Handle "Remember Me" checkbox if needed
                    bool rememberMe = RememberMeCheckBox.IsChecked ?? false;
                    // if (rememberMe) { /* Save credentials logic */ }

                    // Show a welcome message
                    MessageBox.Show($"Welcome, {professor.FirstName}!\nDepartment: {professor.Department}",
                        "Login Successful", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Open the attendance window
                    var attendanceWindow = _serviceProvider.GetRequiredService<AttendanceWindow>();
                    attendanceWindow.Show();

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