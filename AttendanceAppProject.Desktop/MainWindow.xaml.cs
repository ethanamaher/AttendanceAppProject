using AttendanceAppProject.ApiService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace AttendanceAppProject.Desktop
{
    public partial class MainWindow : Window
    {
        private readonly IAttendanceApiClient _apiClient;
        private List<AttendanceRecord> _attendanceRecords;

        public MainWindow(IAttendanceApiClient apiClient)
        {
            InitializeComponent();
            _apiClient = apiClient;
            _attendanceRecords = new List<AttendanceRecord>();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadAttendanceDataAsync();
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            await LoadAttendanceDataAsync();
        }

        private async void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ProfessorNameTextBox.Text))
                {
                    MessageBox.Show("Please enter a Professor name", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                if (ClassComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Please select a Class", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string className = ((ComboBoxItem)ClassComboBox.SelectedItem).Content.ToString();

                var record = new AttendanceRecord
                {
                    ProfessorName = ProfessorNameTextBox.Text,
                    Class = className,
                    CheckInTime = DateTime.Now
                };

                var success = await _apiClient.SubmitAttendanceAsync(record);

                if (success)
                {
                    StatusTextBlock.Text = $"Attendance submission recorded for Professor: {ProfessorNameTextBox.Text} in {className}";
                    await LoadAttendanceDataAsync();

                    // Clear the input fields after successful submission
                    ProfessorNameTextBox.Clear();
                    ProfessorNameTextBox.Focus();
                }
                else
                {
                    StatusTextBlock.Text = "Failed to record attendance submission";
                }
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Error: {ex.Message}";
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task LoadAttendanceDataAsync()
        {
            try
            {
                StatusTextBlock.Text = "Loading attendance data...";
                _attendanceRecords = await _apiClient.GetAttendanceRecordsAsync();

                // Add ordinal numbers
                int counter = 1;
                foreach (var record in _attendanceRecords)
                {
                    record.OrdinalNumber = counter++;
                }

                AttendanceDataGrid.ItemsSource = _attendanceRecords;
                StatusTextBlock.Text = $"Loaded {_attendanceRecords.Count} attendance records";
            }
            catch (Exception ex)
            {
                StatusTextBlock.Text = $"Error: {ex.Message}";
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}