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
          //if all data provided create the class
          if (ProfessorIDTextBox != null && ClassIDTextBox != null && ClassNameTextBox != null)
          {
            ClassDto newClass = new()
            {
              ClassName = ClassNameTextBox.Text,
              ClassNumber = ClassIDTextBox.Text,
              ProfUtdId = ProfessorIDTextBox.Text
            };

          
            var classResponse = await _httpClient.PostAsJsonAsync("api/Class", newClass); // Await the async method

            if (classResponse.IsSuccessStatusCode)
            {
              Debug.WriteLine("Class created");
            }
            else
            {
              Debug.WriteLine($"Failed to create class. Status code: {classResponse.StatusCode}");
            }
          }
          else
          {
            Debug.WriteLine("Missing required fields");
          }
        }
  }
}
