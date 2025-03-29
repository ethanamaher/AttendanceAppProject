using System;
using System.Windows;
namespace AttendanceAppProject.ProfessorLogin
{
    public partial class LoginWindow : Window
    {
        private readonly IProfessorAuthClient _authClient;
        public LoginWindow(IProfessorAuthClient authClient)
        {
            InitializeComponent();
            _authClient = authClient;
            ProfessorIdTextBox.Focus();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Clear previous status
                StatusTextBlock.Text = string.Empty;
                // Validate input
                if (string.IsNullOrWhiteSpace(ProfessorIdTextBox.Text))
                {
                    StatusTextBlock.Text = "Please enter your Professor ID";
                    ProfessorIdTextBox.Focus();
                    return;
                }
                if (string.IsNullOrWhiteSpace(PasswordBox.Password))
                {
                    StatusTextBlock.Text = "Please enter your password";
                    PasswordBox.Focus();
                    return;
                }
                // Disable login button while processing
                IsEnabled = false;
                StatusTextBlock.Text = "Logging in...";
                // Authenticate with the API
                var professor = await _authClient.LoginAsync(
                    ProfessorIdTextBox.Text.Trim(),
                    PasswordBox.Password);
                if (professor != null)
                {
                    // You can handle "Remember Me" logic here if needed
                    // For now, just showing a comment as a placeholder
                    bool rememberMe = RememberMeCheckBox.IsChecked ?? false;
                    // if (rememberMe) { /* Save credentials logic */ }

                    // Show a welcome message
                    MessageBox.Show($"Welcome, {professor.FullName}!\nDepartment: {professor.Department}",
                        "Login Successful", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Launch the main attendance application
                    App.LaunchAttendanceApp();
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
                StatusTextBlock.Text = "Error connecting to server";
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                // Re-enable the login button
                IsEnabled = true;
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