// Chris Palmer
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AttendanceAppProject.Dto.Models;
using AttendanceAppProject.ApiService.Services;
using AttendanceAppProject.ApiService.Data;
using Microsoft.Extensions.DependencyInjection;
using Azure;

namespace AttendanceAppProject.ProfessorLogin
{
  /// <summary>
  /// Interaction logic for ClassCreationWindow.xaml
  /// </summary>
  public partial class ClassCreationWindow : Window
  {
    private List<ClassDto> _professorClassDtos;
    private ProfessorDto _currentProfessor;

    private readonly HttpClient _httpClient;

    public ClassCreationWindow(HttpClient httpClient)
    {
            InitializeComponent();
            _httpClient = httpClient;
    }

        

        private async void CreateClass_Button(object sender, RoutedEventArgs e)
        {
            //get start and end dates
            DateTime? startDate = ClassStartDatePicker.SelectedDate;
            DateTime? endDate = ClassEndDatePicker.SelectedDate;
            
          //if all data provided create the class
          if (ProfessorIDTextBox != null && ClassNumberTextBox != null && ClassNameTextBox != null && endDate != null && startDate != null)
          {
            //if their not null convert to proper type
            DateOnly startDateOnly = DateOnly.FromDateTime((DateTime) startDate);
            DateOnly endDateOnly = DateOnly.FromDateTime((DateTime) endDate);
            
            //create a new class
            Guid tempID = Guid.NewGuid();
            ClassDto newClass = new()
            {
              ClassName = ClassNameTextBox.Text,
              ClassNumber = ClassNumberTextBox.Text,
              ProfUtdId = ProfessorIDTextBox.Text,
              StartDate = startDateOnly,
              EndDate = endDateOnly,
              ClassId = tempID
            };
            var classResponse = await _httpClient.PostAsJsonAsync("api/Class", newClass);

            //submit password for quiz
            PasswordDto newPassword = new()
            {
                ClassId = tempID,
                PasswordId = Guid.NewGuid(),
                PasswordText = ClassPasswordTextBox.Text,
                DateAssigned = DateOnly.FromDateTime(DateTime.Now)
            };
            if (classResponse.IsSuccessStatusCode)
            {
                var passwordResponse = await _httpClient.PutAsJsonAsync($"api/Password/{newPassword.PasswordId}", newPassword);
                    if (passwordResponse.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Class successfully created with password");
                    } else
                    {
                        MessageBox.Show("Password failed to create");
                    }
            }
            else
            {
                    MessageBox.Show("Class failed to create: " + classResponse.ReasonPhrase);
            }
            
            
          }
          else
          {
                
                // Fix for the MessageBox.Show call to resolve CS1503 errors
                try
                {
                  MessageBox.Show("Missing required fields, try again", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch
                {
                  // Suppress errors
                }
                
          }

          //Window.Close();
        }
  }
}
