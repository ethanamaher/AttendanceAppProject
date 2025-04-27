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

namespace AttendanceAppProject.ProfessorLogin
{
  /// <summary>
  /// Interaction logic for ClassCreationWindow.xaml
  /// </summary>
  public partial class ClassCreationWindow : Window
  {
    private List<ClassDto> _professorClassDtos;
    private ProfessorDto _currentProfessor;
        private ServiceProvider serviceProvider;
        private readonly HttpClient _httpClient;
    private readonly ApplicationDbContext _dbContext; // Added field for ApplicationDbContext

    public ClassCreationWindow(IServiceProvider serviceProvider, ApplicationDbContext dbContext)
    {
      InitializeComponent();
      _dbContext = dbContext; // Initialize the ApplicationDbContext
    }

        public ClassCreationWindow(ServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
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

        ClassService classService = new(serviceProvider,_dbContext); // Pass the required ApplicationDbContext
        var classResponse = await classService.AddClassAsync(newClass); // Await the async method
        if (classResponse != null)
        {
          Debug.WriteLine("Class created");
        }
      }
      else
      {
        Debug.WriteLine("Missing required fields");
      }
    }
  }
}
