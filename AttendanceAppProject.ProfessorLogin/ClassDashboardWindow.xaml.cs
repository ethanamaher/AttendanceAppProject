//Chris Palmer

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using AttendanceAppProject.Dto.Models;
using Microsoft.Extensions.DependencyInjection;
using Azure;
using AttendanceAppProject.ApiService.Data.Models;
using System.IO;

namespace AttendanceAppProject.ProfessorLogin
{
    /// <summary>
    /// Interaction logic for ClassDashboardWindow.xaml
    /// </summary>
    public partial class ClassDashboardWindow : Window
    {
        private readonly IServiceProvider _serviceProvider;
        private List<ClassScheduleDto> _classScheduleDtos;
        private List<ClassDto> _professorClassDtos;
        private List<ClassDto> _filteredClassDtos;
        private ProfessorDto? _currentProfessor;
        private readonly HttpClient _httpClient;

        public ClassDashboardWindow(HttpClient httpClient)
        {
            InitializeComponent();
            _httpClient = httpClient;
        }
        private async void Window_Loaded(object sender, RoutedEventArgs e)
        { 
            _currentProfessor = App.CurrentProfessor;
            await LoadProfessorClassDtos();


        }

        private async Task LoadProfessorClassDtos()
        {
            Debug.WriteLine("Class Dashboard");
            Debug.WriteLine($"Loading the classes for {_currentProfessor}");
            try
            {
                if (_currentProfessor == null) return;

                var response = await _httpClient.GetAsync($"api/Class/professor/{_currentProfessor.UtdId}");

                if (response.IsSuccessStatusCode)
                {
                    _professorClassDtos = await response.Content.ReadFromJsonAsync<List<ClassDto>>();

                    Debug.WriteLine($"Loaded {_professorClassDtos.Count} classes for professor {_currentProfessor?.UtdId}, dashboard");
                    foreach (var c in _professorClassDtos)
                    {
                        Debug.WriteLine($"Class: {c.ClassName} - {c.ClassPrefix} {c.ClassNumber}");
                    }

                    // Clear and add the "All ClassDtos" option
                    ClassSelectionBox.Items.Clear();
                    ClassSelectionBox.Items.Add(new ComboBoxItem { Content = "All Classes" });

                    // Add each class to selection filter
                    foreach (var classItem in _professorClassDtos)
                    {
                        ClassSelectionBox.Items.Add(new ComboBoxItem
                        {
                            Content = $"{classItem.ClassPrefix} {classItem.ClassNumber}: {classItem.ClassName}",
                            Tag = classItem.ClassId,
                            
                        });
                    }

                    // Select "All Classes" by default
                    ClassSelectionBox.SelectedIndex = 0;


                    //draw class rows to the datagrid table
                    if (ClassDataGrid != null)
                    {
                        ClassDataGrid.ItemsSource = null;
                        ClassDataGrid.ItemsSource = _professorClassDtos;
                    }
                }
                else
                {
                    Debug.WriteLine($"Failed to load classes. Status: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading classes: {ex.Message}");
            }
        }

        private void ClassSelectionBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        // Button to open edit password window
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Guid selectedClassId = (Guid) ((Button) sender).Tag;
            ClassDto _currentClass;

            // for each class in the database
            foreach (var classItem in _professorClassDtos)
            {
                //if the class id matches the selected class id
                if ((Guid) classItem.ClassId == selectedClassId)
                {
                    _currentClass = classItem;
                    Debug.WriteLine($"Class: {_currentClass.ClassId}");

                    bool windowIsOpen = false;
                    foreach(Window openWindow in Application.Current.Windows)
                    {
                        // if editpass window is already open, bring it to front
                        if(openWindow is EditPass)
                        {
                            windowIsOpen = true;
                            openWindow.Activate();
                            break;
                        }
                    }

                    if (!windowIsOpen)
                    {
                        //create edit password window
                        if (App.Current is App app && app.ServiceProvider != null)
                        {
                            try
                            {
                                var editPassWindow = app.ServiceProvider.GetRequiredService<EditPass>();
                                editPassWindow.SetProfessor((Guid)_currentClass.ClassId, _currentProfessor);
                                editPassWindow.Show();
                            } catch (Exception ex) {
                                Debug.WriteLine("Error creating EditPass window");
                            }
                        } else
                        {
                            Debug.WriteLine("Error opening EditPass window");
                        }
                    }
                    
                }
                
            }
            

        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Guid selectedClassId = (Guid)((Button)sender).Tag;
            ClassDto _currentClass;

            // for each class in the database
            foreach (var classItem in _professorClassDtos)
            {
              //if the class id matches the selected class id
              if ((Guid)classItem.ClassId == selectedClassId)
              {
                _currentClass = classItem;
                // Show a Yes/No message box
                var result = MessageBox.Show(
                    $"Are you sure you want to delete {classItem.ClassName}? This action cannot be undone.",
                    "Confirmation", // Added a title string here to fix the error
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning
                );

                // Check the user's response
                if (result == MessageBoxResult.Yes)
                {
                  var deleteResponse = await _httpClient.DeleteAsync($"/api/class/{classItem.ClassId}");
                  Debug.WriteLine($"Class {classItem.ClassName} deleted.");
                }
                else
                {
                  Debug.WriteLine($"Class {classItem.ClassName} deletion canceled.");
                }
              }
            }
        }

        /* Method to import tab-delimited file and add students to database
         * Written by Maaz Raza
         */
        private async void ImportStudents_Click(object sender, RoutedEventArgs e)
        {
            // Prompt user to select a file
            var openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // Try to get the selected class
                if (ClassSelectionBox.SelectedItem is ComboBoxItem selectedItem &&
                    selectedItem.Tag is Guid selectedClassId)
                {
                    try
                    {
                        var lines = await File.ReadAllLinesAsync(openFileDialog.FileName);
                        foreach (var line in lines.Skip(1)) // skip header
                        {
                            var columns = line.Split('\t');
                            if (columns.Length >= 4)
                            {
                                var studentDto = new StudentDto
                                {
                                    LastName = columns[0].Trim(),
                                    FirstName = columns[1].Trim(),
                                    Username = columns[2].Trim(),
                                    UtdId = columns[3].Trim()
                                };

                                // 1. Add student
                                var studentResponse = await _httpClient.PostAsJsonAsync("api/Student", studentDto);

                                if (studentResponse.IsSuccessStatusCode)
                                {
                                    Debug.WriteLine($"Student {studentDto.UtdId} added.");

                                    // 2. Enroll student in class
                                    var studentClassDto = new StudentClassDto
                                    {
                                        StudentId = studentDto.UtdId,
                                        ClassId = selectedClassId
                                    };

                                    var enrollmentResponse = await _httpClient.PostAsJsonAsync("api/StudentClass", studentClassDto);
                                    if (enrollmentResponse.IsSuccessStatusCode)
                                    {
                                        Debug.WriteLine($"Student {studentDto.UtdId} enrolled in class {selectedClassId}.");
                                    }
                                    else
                                    {
                                        Debug.WriteLine($"Enrollment failed for {studentDto.UtdId}: {enrollmentResponse.StatusCode}");
                                    }
                                }
                                else
                                {
                                    Debug.WriteLine($"Student insert failed for {studentDto.UtdId}: {studentResponse.StatusCode}");
                                }
                            }
                        }

                        MessageBox.Show("Students imported and enrolled successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Import Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Please select a class before importing students.", "Class Required", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
        }
    }
}
