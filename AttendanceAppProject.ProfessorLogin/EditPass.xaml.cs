//Chris Palmer
using System;
using System.CodeDom;
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
using System.Windows.Shapes;
using AttendanceAppProject.ApiService.Data.Models;
using AttendanceAppProject.ApiService.Services;
using AttendanceAppProject.Dto.Models;
using Azure;
using Microsoft.UI.Xaml.Controls;

namespace AttendanceAppProject.ProfessorLogin
{
    /// <summary>
    /// Interaction logic for EditPass.xaml
    /// </summary>
    public partial class EditPass : Window
    {
        private HttpClient _http;
        private  Guid _classId;
        private ProfessorDto _currentProfessor;
        public EditPass()
        {
            InitializeComponent();
            _http = new HttpClient();
            _http.BaseAddress = new Uri("https://localhost:7530/");
        }
        public void SetProfessor(Guid classId, ProfessorDto professor)
        {
            _classId = classId;
            _currentProfessor = professor;
        }
        private async void EditPass_Submit(object sender, RoutedEventArgs e)
        {
            //PasswordService passService = new PasswordService( context);
            //update password
            PasswordDto oldPassword = new PasswordDto();
            oldPassword.PasswordText = OldPassField.Text;
            oldPassword.ClassId = _classId;
          var updatedPassword = new PasswordDto
          {
            ClassId = _classId,
            PasswordText = NewPassField.Text, // Replace with the new password
            DateAssigned = DateOnly.FromDateTime(DateTime.Now) // Set the current date
          };


          try
          {
            //verify old password
            var validateResponse = await _http.PostAsJsonAsync($"api/Password/validate/", oldPassword);
                if (!validateResponse.IsSuccessStatusCode)
                {
                    MessageBox.Show($"Failed to validate old password");
                }
                else
                {
                    MessageBox.Show($"Old password found for class {oldPassword.ClassId}");
                }
                
                Debug.WriteLine($"HERHEHRE s{_classId}");
            //attempt to update password
            var response = await _http.PutAsJsonAsync($"api/password/{_classId}/", updatedPassword);
            if (!response.IsSuccessStatusCode)
            {
              MessageBox.Show($"Failed to update pass: {response.ReasonPhrase}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
              System.Diagnostics.Debug.WriteLine(response.ToString());
              return;
            }
            else
            {
                MessageBox.Show($"Password Changed Successfully!");
            }
          }
          catch (Exception ex)
          {
            MessageBox.Show($"An exception occurred: {ex.Message}", "Exception", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
          }

          MessageBox.Show($"Password updated to {NewPassField.Text}");
        }
    }
}
