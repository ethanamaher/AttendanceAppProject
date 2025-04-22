using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using AttendanceAppProject.Dto.Models;
using AttendanceAppProject.ProfessorLogin.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Data;
using System.ComponentModel;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace AttendanceAppProject.ProfessorLogin
{
    public partial class AttendanceWindow : Window
    {
        private List<AttendanceInstanceDto> _allAttendanceInstanceDtos;
        private List<AttendanceInstanceDto> _filteredRecords;
        private List<ClassDto> _professorClassDtos;
        private ProfessorDto _currentProfessor;

        private readonly HttpClient _httpClient;

        public AttendanceWindow(HttpClient httpClient)
        {
            Debug.WriteLine("prof from dash: " + App.CurrentProfessor.FirstName + " " + App.CurrentProfessor.LastName);
            Debug.WriteLine("Dashboard init");
            InitializeComponent();
            _allAttendanceInstanceDtos = new List<AttendanceInstanceDto>();
            _filteredRecords = new List<AttendanceInstanceDto>();
            _professorClassDtos = new List<ClassDto>();
            _httpClient = httpClient;

            // Set default sorting if the control exists
            if (SortByComboBox != null && SortByComboBox.Items.Count > 0)
            {
                SortByComboBox.SelectedIndex = 0;
            }
        }
        //object sender, RoutedEventArgs e
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("Window Loaded");
            try
            {
                this.Title = $"Student Attendance Database - {App.CurrentProfessor.FirstName} {App.CurrentProfessor.LastName}";

                // Fetch professor data from the API
                await GetProfessorFromApiAsync(App.CurrentProfessor.UtdId);

                // Load professor's classes
                await LoadProfessorClassDtos();

                // Load attendance data
                await LoadAttendanceData();
                
                // Display professor information
                _currentProfessor = App.CurrentProfessor;

                if (_currentProfessor != null)
                {
                    ProfessorNameTextBlock.Text = $"Welcome, {_currentProfessor.FirstName} {_currentProfessor.LastName}";
                    DepartmentTextBlock.Text = $"Department: {_currentProfessor.Department}";
                    this.Title = $"Student Attendance Database - {_currentProfessor.FirstName} {_currentProfessor.LastName}";

                    await GetProfessorFromApiAsync(_currentProfessor.UtdId);
                    await LoadProfessorClassDtos();
                    await LoadAttendanceData();

                    StatusTextBlock.Text = "Data loaded successfully";
                    LastUpdatedTextBlock.Text = $"Last updated: {DateTime.Now:g}";
                }
                else
                {
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

        private async Task GetProfessorFromApiAsync(string professorId)
        {
            try
            {
                var response = await _httpClient.GetAsync("api/Professor");
                if (response.IsSuccessStatusCode)
                {
                    var professors = await response.Content.ReadFromJsonAsync<List<ProfessorDto>>();
                    _currentProfessor = professors.FirstOrDefault(p => p.UtdId == professorId);

                    if (_currentProfessor == null)
                    {
                        // Fallback to mock data for testing
                        _currentProfessor = new ProfessorDto
                        {
                            UtdId = professorId,
                            FirstName = "Mock",
                            LastName = "Professor"
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error getting professor: {ex.Message}");
                // Create a mock professor for testing
                _currentProfessor = new ProfessorDto
                {
                    UtdId = professorId,
                    FirstName = "Mock",
                    LastName = "Professor"
                };
            }
        }

        private async Task LoadProfessorClassDtos()
        {
            System.Diagnostics.Debug.WriteLine($"Loading the classes for {_currentProfessor}");
            try
            {
                if (_currentProfessor == null) return;

                var response = await _httpClient.GetAsync($"api/Class/professor/{_currentProfessor.UtdId}");

                if (response.IsSuccessStatusCode)
                {
                    _professorClassDtos = await response.Content.ReadFromJsonAsync<List<ClassDto>>();

                    System.Diagnostics.Debug.WriteLine($"Loaded {_professorClassDtos.Count} classes for professor {_currentProfessor?.UtdId}");
                    foreach (var c in _professorClassDtos)
                    {
                        System.Diagnostics.Debug.WriteLine($"Class: {c.ClassId} - {c.ClassPrefix} {c.ClassNumber}");
                    }

                    // Clear and add the "All ClassDtos" option
                    ClassComboBox.Items.Clear();
                    ClassComboBox.Items.Add(new ComboBoxItem { Content = "All Classes" });

                    // Add each class
                    foreach (var classItem in _professorClassDtos)
                    {
                        ClassComboBox.Items.Add(new ComboBoxItem
                        {
                            Content = $"{classItem.ClassPrefix} {classItem.ClassNumber}: {classItem.ClassName}",
                            Tag = classItem.ClassId
                        });
                    }

                    // Select "All ClassDtos" by default
                    ClassComboBox.SelectedIndex = 0;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to load classes. Status: {response.StatusCode}");
                    // If no classes found, populate with mock data for testing
                    PopulateWithMockClassDtos();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading classes: {ex.Message}");
                // Populate with mock data for testing
                PopulateWithMockClassDtos();
            }
        }

        private void PopulateWithMockClassDtos()
        {
            // Clear and add mock classes for testing
            ClassComboBox.Items.Clear();
            ClassComboBox.Items.Add(new ComboBoxItem { Content = "All ClassDtos" });
            ClassComboBox.Items.Add(new ComboBoxItem { Content = "CS 4485: Senior Design", Tag = Guid.NewGuid() });
            ClassComboBox.Items.Add(new ComboBoxItem { Content = "CS 3354: Software Engineering", Tag = Guid.NewGuid() });
            ClassComboBox.Items.Add(new ComboBoxItem { Content = "CS 2336: Computer Science II", Tag = Guid.NewGuid() });

            // Select "All ClassDtos" by default
            ClassComboBox.SelectedIndex = 0;
        }

        private async Task LoadAttendanceData()
        {
            try
            {
                // First, attempt to retrieve data from the API
                if (_currentProfessor != null)
                {
                    List<AttendanceInstanceDto> records = new List<AttendanceInstanceDto>();
                    bool useApiData = false;

                    try
                    {
                        foreach (var classItem in _professorClassDtos)
                        {
                            System.Diagnostics.Debug.WriteLine($"Attempting to fetch attendance for class: {classItem.ClassId}");
                            var response = await _httpClient.GetAsync($"api/AttendanceInstance/class/{classItem.ClassId}");

                            if (response.IsSuccessStatusCode)
                            {
                                var attendanceInstances = await response.Content.ReadFromJsonAsync<List<AttendanceInstanceDto>>();

                                // For each attendance instance, create a record
                                int index = 1;
                                foreach (var instance in attendanceInstances)
                                {
                                    var record = instance;

                                    records.Add(record);
                                }

                                useApiData = true;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"API error: {ex.Message}. Falling back to mock data.");
                        useApiData = false;
                    }

                    if (useApiData && records.Count > 0)
                    {
                        _allAttendanceInstanceDtos = records;
                    }
                    else
                    {
                        // Fallback to mock data
                        _allAttendanceInstanceDtos = MockDataProvider.GenerateMockAttendanceData(
                            _currentProfessor.UtdId,
                            $"{_currentProfessor.FirstName} {_currentProfessor.LastName}");
                    }

                    // Apply initial filtering and sorting
                    ApplyFilters();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading attendance data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Fallback to mock data
                _allAttendanceInstanceDtos = MockDataProvider.GenerateMockAttendanceData(
                    _currentProfessor?.UtdId ?? "unknown",
                    _currentProfessor != null ? $"{_currentProfessor.FirstName} {_currentProfessor.LastName}" : "Unknown Professor");

                ApplyFilters();
            }
        }

        private void ApplyFilters()
        {
            try
            {
                // Make sure we have attendance records to filter
                if (_allAttendanceInstanceDtos == null)
                {
                    _allAttendanceInstanceDtos = new List<AttendanceInstanceDto>();
                }

                // Create a new filtered list from all records
                _filteredRecords = new List<AttendanceInstanceDto>(_allAttendanceInstanceDtos);

                if (ClassComboBox != null && ClassComboBox.SelectedIndex > 0) // Skip "All Classes"
                {
                    var selectedItem = ClassComboBox.SelectedItem as ComboBoxItem;
                    if (selectedItem != null && selectedItem.Tag is Guid selectedClassId)
                    {
                        _filteredRecords = _filteredRecords
                            .Where(r => r.ClassId == selectedClassId)
                            .ToList();
                    }
                }

                // Apply single date filter if selected
                if (DateFilterPicker != null && DateFilterPicker.SelectedDate.HasValue)
                {
                    DateTime selectedDate = DateFilterPicker.SelectedDate.Value;
                    _filteredRecords = _filteredRecords.Where(r =>
                        r.DateTime == selectedDate.Date).ToList();
                }

                // Apply date range filter if both dates are selected
                else if (StartDatePicker != null && EndDatePicker != null &&
                         StartDatePicker.SelectedDate.HasValue && EndDatePicker.SelectedDate.HasValue)
                {
                    DateTime startDate = StartDatePicker.SelectedDate.Value;
                    DateTime endDate = EndDatePicker.SelectedDate.Value.AddDays(1).AddSeconds(-1); // End of day

                    _filteredRecords = _filteredRecords.Where(r =>
                        r.DateTime >= startDate && r.DateTime <= endDate).ToList();

                    if (DateRangeTextBlock != null)
                    {
                        DateRangeTextBlock.Text = $"Date range: {startDate.ToShortDateString()} - {EndDatePicker.SelectedDate.Value.ToShortDateString()}";
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
                /*
                int counter = 1;
                foreach (var record in _filteredRecords)
                {
                    record.OrdinalNumber = counter++;
                }
                */

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
                        _filteredRecords = _filteredRecords.OrderByDescending(r => r.DateTime).ToList();
                        break;
                    case "Sort by Date (Old-New)":
                        _filteredRecords = _filteredRecords.OrderBy(r => r.DateTime).ToList();
                        break;
                    case "Sort by Student ID":
                        _filteredRecords = _filteredRecords.OrderBy(r => r.StudentId).ToList();
                        break;
                    case "Sort by ClassDto":
                        _filteredRecords = _filteredRecords.OrderBy(r => r.ClassId).ThenBy(r => r.StudentId).ToList();
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
                    .GroupBy(r => r.ClassId)
                    .Select(g => new { ClassDto = g.Key, Count = g.Count() })
                    .OrderByDescending(x => x.Count);

                string distribution = "ClassDto distribution:\n";
                foreach (var item in classDistribution)
                {
                    distribution += $"- {item.ClassDto}: {item.Count} records\n";
                }

                ClassDistributionTextBlock.Text = distribution;
            }
            catch (Exception ex)
            {
                // Log the exception but don't show another message box to avoid cascading errors
                Console.WriteLine($"Error in UpdateStatistics: {ex.Message}");
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Show loading message
                if (StatusTextBlock != null)
                {
                    StatusTextBlock.Text = "Refreshing data...";
                }

                // Refresh data
                await LoadProfessorClassDtos();
                await LoadAttendanceData();

                // Update status
                if (StatusTextBlock != null)
                {
                    StatusTextBlock.Text = "Data refreshed successfully";
                }

                // Update last updated timestamp
                if (LastUpdatedTextBlock != null)
                {
                    LastUpdatedTextBlock.Text = $"Last updated: {DateTime.Now.ToString("g")}";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error refreshing data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClassDtoComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
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

        // define this properly later on - just added it here for now to avoid an error
        private void ClassComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyFilters();
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
                if (sender is Button button && button.DataContext is AttendanceInstanceDto record)
                {
                    // Show details in a message box
                    MessageBox.Show(
                        $"Student ID: {record.StudentId}\n" +
                        $"ClassDto: {record.ClassId}\n" +
                        $"Check-in Time: {record.DateTime}\n",
                        // can retrieve this data via an api call later on
                        // $"Quiz Question: {record.QuizQuestion}\n" +
                        // $"Quiz Answer: {record.QuizAnswer}\n" +
                        // $"Late: {(record.IsLate ? "Yes" : "No")}",
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

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Get the clicked record
                if (sender is Button button && button.DataContext is AttendanceInstanceDto record)
                {
                    // In a real application, you would open a dialog to edit the record
                    // For now, let's demonstrate API integration by toggling the IsLate flag

                    // Update the UI record
                    record.IsLate = !record.IsLate;

                    // Find the class ID for this record
                    Guid? classId = null;
                    foreach (ComboBoxItem item in ClassComboBox.Items)
                    {
                        if (item.Content.ToString() == record.ClassId.ToString() && item.Tag is Guid id)
                        {
                            classId = id;
                            break;
                        }
                    }

                    if (classId.HasValue)
                    {
                        try
                        {
                            // Try to update via API
                            var dto = new
                            {
                                StudentId = record.StudentId,
                                ClassDtoId = classId.Value,
                                IsLate = record.IsLate,
                                ExcusedAbsence = false,
                                DateTime = record.DateTime
                            };

                            var response = await _httpClient.PostAsJsonAsync("api/AttendanceInstance/absent-or-late", dto);

                            if (response.IsSuccessStatusCode)
                            {
                                StatusTextBlock.Text = "Record updated successfully";
                            }
                            else
                            {
                                StatusTextBlock.Text = "Failed to update record via API";
                            }
                        }
                        catch (Exception apiEx)
                        {
                            System.Diagnostics.Debug.WriteLine($"API error during edit: {apiEx.Message}");
                        }
                    }

                    // Refresh the DataGrid
                    AttendanceDataGrid.Items.Refresh();

                    // Show a message about the edit
                    MessageBox.Show(
                        $"Updated record for Student ID: {record.StudentId}\n" +
                        $"Changed 'Late' status to: {((bool)record.IsLate ? "Yes" : "No")}",
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
                // Should access static property with class name, not instance
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