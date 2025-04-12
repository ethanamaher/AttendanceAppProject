// Canh Nguyen 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AttendanceAppProject.ProfessorLogin.Models;
using AttendanceAppProject.ProfessorLogin.Services;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Windows.Data;
using System.ComponentModel;
using System.Net.Http;
using AttendanceAppProject.ApiService.Models;
using AttendanceAppProject.ProfessorLogin;
using System.Net.Http.Json;
using Azure;

namespace AttendanceAppProject.ProfessorLogin
{
    public partial class AttendanceWindow : Window
    {
        private List<AttendanceAppProject.ProfessorLogin.Models.AttendanceRecord> _allAttendanceRecords;
        private List<AttendanceAppProject.ProfessorLogin.Models.AttendanceRecord> _filteredRecords;

        private HttpClient _httpClient;
        public AttendanceWindow()
        {
            InitializeComponent();
            _allAttendanceRecords = new List<AttendanceAppProject.ProfessorLogin.Models.AttendanceRecord>();
            _filteredRecords = new List<AttendanceAppProject.ProfessorLogin.Models.AttendanceRecord>();
            _httpClient = new HttpClient();
            

            // Set default sorting if the control exists
            if (SortByComboBox != null && SortByComboBox.Items.Count > 0)
            {
                SortByComboBox.SelectedIndex = 0;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Window Loaded");
            try
            {
                // Display professor information
                if (App.Current.Properties["CurrentProfessor"] is Professor currentProfessor)
                {
                    if (ProfessorNameTextBlock != null)
                    {
                        ProfessorNameTextBlock.Text = $"Welcome, {currentProfessor.FullName}";
                    }

                    if (DepartmentTextBlock != null)
                    {
                        DepartmentTextBlock.Text = $"Department: {currentProfessor.Department}";
                    }

                    // Set window title
                    this.Title = $"Student Attendance Database - {currentProfessor.FullName}";

                    // Select first item in the Class combobox if it exists
                    if (ClassComboBox != null && ClassComboBox.Items.Count > 0)
                    {
                        ClassComboBox.SelectedIndex = 0;
                    }

                    // Load attendance data
                    LoadAttendanceData();
                }
                else
                {
                    // This shouldn't happen, but handle it gracefully
                    MessageBox.Show("No professor data available. Please log in again.",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    ShowLoginWindow();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Declare HTTP Client and list of students
        //private HttpClient _httpClient;
        private List<StudentDto> students = new();

        private async void LoadAttendanceData()
        {
            try
            {
                // Use the shared HttpClient instance
                var response = await _httpClient.GetAsync("api/student");

                if (response.IsSuccessStatusCode)
                {
                    var studentList = await response.Content.ReadFromJsonAsync<List<StudentDto>>();
                    students = studentList ?? new List<StudentDto>();

                    //// Update UI (dispatch to UI thread if needed)
                    //Dispatcher.Invoke(() => 
                    //{
                    //    dataGrid.ItemsSource = students; // Replace with your actual UI control
                    //});
                }
                else
                {
                    MessageBox.Show($"API Error: {response.StatusCode}", "Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Network error: {ex.Message}\n\nIs the API running?",
                                "Connection Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unexpected error: {ex.Message}",
                                "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // HTTP Get student data from database
        protected async Task LoadStudentsAsync()
        {
            students = await _httpClient.GetFromJsonAsync<List<StudentDto>>("api/student/");
            System.Diagnostics.Debug.WriteLine("yo" + students);
        }

        private void ApplyFilters()
        {
            try
            {
                // Make sure we have attendance records to filter
                if (_allAttendanceRecords == null)
                {
                    _allAttendanceRecords = new List<AttendanceAppProject.ProfessorLogin.Models.AttendanceRecord>();
                }
        
                // Create a new filtered list from all records
                _filteredRecords = new List<AttendanceAppProject.ProfessorLogin.Models.AttendanceRecord>(_allAttendanceRecords);
        
                // Apply class filter if selected
                if (ClassComboBox != null && ClassComboBox.SelectedIndex > 0) // Skip "All Classes"
                {
                    var selectedItem = ClassComboBox.SelectedItem as ComboBoxItem;
                    if (selectedItem != null)
                    {
                        string selectedClass = selectedItem.Content?.ToString() ?? string.Empty;
                        _filteredRecords = _filteredRecords.Where(r => r.Class == selectedClass).ToList();
                    }
                }
        
                // Apply single date filter if selected
                if (DateFilterPicker != null && DateFilterPicker.SelectedDate.HasValue)
                {
                    DateTime selectedDate = DateFilterPicker.SelectedDate.Value;
                    _filteredRecords = _filteredRecords.Where(r =>
                        r.CheckInTime.Date == selectedDate.Date).ToList();
                }
        
                // Apply date range filter if both dates are selected
                else if (StartDatePicker != null && EndDatePicker != null &&
                         StartDatePicker.SelectedDate.HasValue && EndDatePicker.SelectedDate.HasValue)
                {
                    DateTime startDate = StartDatePicker.SelectedDate.Value;
                    DateTime endDate = EndDatePicker.SelectedDate.Value.AddDays(1).AddSeconds(-1); // End of day
        
                    _filteredRecords = _filteredRecords.Where(r =>
                        r.CheckInTime >= startDate && r.CheckInTime <= endDate).ToList();
        
                    if (DateRangeTextBlock != null)
                    {
                        DateRangeTextBlock.Text = $"Date range: {startDate.ToShortDateString()} -       {EndDatePicker.SelectedDate.Value.ToShortDateString()}";
                    }
                }
                else
                {
                    if (DateRangeTextBlock != null)
                    {
                        DateRangeTextBlock.Text = "Date range: All dates";
                    }
                }
        
                // Apply student ID filter if provided
                if (StudentIdTextBox != null && !string.IsNullOrWhiteSpace(StudentIdTextBox.Text))
                {
                    string studentId = StudentIdTextBox.Text.Trim();
                    _filteredRecords = _filteredRecords.Where(r =>
                        r.StudentId.Contains(studentId, StringComparison.OrdinalIgnoreCase)).ToList();
                }
        
                // Apply current sorting
                ApplySorting();
        
                // Update ordinal numbers
                int counter = 1;
                foreach (var record in _filteredRecords)
                {
                    record.OrdinalNumber = counter++;
                }
        
                // Update the DataGrid
                if (AttendanceDataGrid != null)
                {
                    AttendanceDataGrid.ItemsSource = null;
                    AttendanceDataGrid.ItemsSource = _filteredRecords;
                }
        
                // Update stats
                UpdateStatistics();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying filters: {ex.Message}\n\nStack Trace: {ex.StackTrace}",
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void ApplySorting()
        {
            try
            {
                if (_filteredRecords == null)
                {
                    return;
                }
        
                if (SortByComboBox == null || SortByComboBox.SelectedIndex < 0)
                {
                    return;
                }
        
                var selectedItem = SortByComboBox.SelectedItem as ComboBoxItem;
                if (selectedItem == null)
                {
                    return;
                }
        
                string sortOption = selectedItem.Content?.ToString();
                if (string.IsNullOrEmpty(sortOption))
                {
                    return;
                }
        
                switch (sortOption)
                {
                    case "Sort by Date (New-Old)":
                        _filteredRecords = _filteredRecords.OrderByDescending(r => r.CheckInTime).ToList();
                        break;
                    case "Sort by Date (Old-New)":
                        _filteredRecords = _filteredRecords.OrderBy(r => r.CheckInTime).ToList();
                        break;
                    case "Sort by Student ID":
                        _filteredRecords = _filteredRecords.OrderBy(r => r.StudentId).ToList();
                        break;
                    case "Sort by Class":
                        _filteredRecords = _filteredRecords.OrderBy(r => r.Class).ThenBy(r => r.StudentId).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // Log the exception but don't show another message box to avoid cascading errors
                Console.WriteLine($"Error in ApplySorting: {ex.Message}");
            }
        }
        
        private void UpdateStatistics()
        {
            try
            {
                // Make sure we have records and UI elements
                if (_filteredRecords == null || RecordCountTextBlock == null || ClassDistributionTextBlock == null)
                {
                    return;
                }
        
                // Update record count
                RecordCountTextBlock.Text = $"{_filteredRecords.Count} records found";
        
                // Update class distribution
                var classDistribution = _filteredRecords
                    .GroupBy(r => r.Class)
                    .Select(g => new { Class = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count);
        
                string distribution = "Class distribution:\n";
                foreach (var item in classDistribution)
                {
                    distribution += $"- {item.Class}: {item.Count} records\n";
                }
        
                ClassDistributionTextBlock.Text = distribution;
            }
            catch (Exception ex)
            {
                // Log the exception but don't show another message box to avoid cascading errors
                Console.WriteLine($"Error in UpdateStatistics: {ex.Message}");
            }
        }
        
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadAttendanceData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void ClassComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing class filter: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void DateFilterPicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                // Clear date range if single date is selected
                if (DateFilterPicker != null && DateFilterPicker.SelectedDate.HasValue)
                {
                    if (StartDatePicker != null)
                    {
                        StartDatePicker.SelectedDate = null;
                    }
        
                    if (EndDatePicker != null)
                    {
                        EndDatePicker.SelectedDate = null;
                    }
                }
        
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing date filter: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void ApplyDateRangeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Clear single date if date range is applied
                if (StartDatePicker != null && EndDatePicker != null &&
                    StartDatePicker.SelectedDate.HasValue && EndDatePicker.SelectedDate.HasValue)
                {
                    if (DateFilterPicker != null)
                    {
                        DateFilterPicker.SelectedDate = null;
                    }
        
                    ApplyFilters();
                }
                else
                {
                    MessageBox.Show("Please select both start and end dates for the range filter.",
                        "Date Range Selection", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error applying date range: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void SearchStudentButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error searching for student: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void SortByComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                ApplyFilters();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error changing sort order: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void ClearFiltersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Reset all filters
                if (ClassComboBox != null && ClassComboBox.Items.Count > 0)
                {
                    ClassComboBox.SelectedIndex = 0;
                }
        
                if (DateFilterPicker != null)
                {
                    DateFilterPicker.SelectedDate = null;
                }
        
                if (StartDatePicker != null)
                {
                    StartDatePicker.SelectedDate = null;
                }
        
                if (EndDatePicker != null)
                {
                    EndDatePicker.SelectedDate = null;
                }
        
                if (StudentIdTextBox != null)
                {
                    StudentIdTextBox.Text = string.Empty;
                }
        
                // Reset sorting to default
                if (SortByComboBox != null && SortByComboBox.Items.Count > 0)
                {
                    SortByComboBox.SelectedIndex = 0;
                }
        
                // Reapply filters (which will now show all records)
                ApplyFilters();
        
                if (StatusTextBlock != null)
                {
                    StatusTextBlock.Text = "All filters cleared";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error clearing filters: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the clicked record
                
                if (sender is Button button && button.DataContext is AttendanceAppProject.ProfessorLogin.Models.AttendanceRecord record)
                {
                    // Show details in a message box
                    MessageBox.Show(
                        $"Student ID: {record.StudentId}\n" +
                        $"Class: {record.Class}\n" +
                        $"Check-in Time: {record.CheckInTime}\n" +
                        $"Quiz Question: {record.QuizQuestion}\n" +
                        $"Quiz Answer: {record.QuizAnswer}\n" +
                        $"Late: {(record.IsLate ? "Yes" : "No")}",
                        "Attendance Record Details",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error showing details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the clicked record
                if (sender is Button button && button.DataContext is AttendanceAppProject.ProfessorLogin.Models.AttendanceRecord record)
                {
                    // In a real application, you would open a dialog to edit the record
                    // For this example, we'll just show a message
                    MessageBox.Show(
                        $"Editing record for Student ID: {record.StudentId}\n" +
                        "In a real application, this would open an edit dialog.",
                        "Edit Attendance Record",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing record: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void ExportToExcelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // In a real application, you would implement exporting to Excel here
                // For this example, we'll just show a message
                MessageBox.Show(
                    "This would export the current filtered records to an Excel file.\n" +
                    "You would need to add a library like EPPlus or ClosedXML to implement this feature.",
                    "Export to Excel",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
        
                if (StatusTextBlock != null)
                {
                    StatusTextBlock.Text = "Export to Excel feature demonstrated";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error with export operation: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void PrintReportButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // In a real application, you would implement printing here
                // For this example, we'll just show a message
                MessageBox.Show(
                    "This would print a report of the current filtered records.\n" +
                    "You would need to create a print document and use PrintDialog to implement this feature.",
                    "Print Report",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);
        
                if (StatusTextBlock != null)
                {
                    StatusTextBlock.Text = "Print Report feature demonstrated";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error with print operation: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Clear the current professor data
                App.CurrentProfessor = null;
        
                // Show the login window
                ShowLoginWindow();
        
                // Close this window
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error during logout: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private void ShowLoginWindow()
        {
            try
            {
                // Get the service provider
                if (App.Current is App app && app.ServiceProvider != null)
                {
                    // Create a new login window
                    var loginWindow = app.ServiceProvider.GetRequiredService<LoginWindow>();
                    loginWindow.Show();
                }
                else
                {
                    // Fallback if service provider is not available
                    var loginWindow = new LoginWindow(new ServiceCollection().BuildServiceProvider());
                    loginWindow.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error opening login window: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}