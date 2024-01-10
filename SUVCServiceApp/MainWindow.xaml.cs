using Newtonsoft.Json;
using SUVCServiceApp.ViewModel;
using SUVCServiceApp.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
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

namespace SUVCServiceApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private async void buttonAuthorization_Click(object sender, RoutedEventArgs e)
        {
            string login = textBoxLogin.Text;
            string password = textBoxPassword.Password;
            try
            {

                using (HttpClient client = new HttpClient())
                {
                    string apiUrl = $"http://ppapr4-001-site1.itempurl.com/api/Users?login={login}&password={password}";

                    HttpResponseMessage response = await client.GetAsync(apiUrl);

                    if (response.IsSuccessStatusCode)
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        ResponseUsers authenticatedUser = JsonConvert.DeserializeObject<ResponseUsers>(responseData);
                        if (authenticatedUser.Role == "Администратор")
                        {
                            new AdministratorWindow(authenticatedUser).Show();
                            Close();
                        }
                        else if (authenticatedUser.Role == "ИТ-Отдел")
                        {
                            new EmployeeITWindow(authenticatedUser).Show();
                            Close();
                        }
                        else if (authenticatedUser.Role == "Сотрудник")
                        {
                            new EmployeeWindow(authenticatedUser).Show();
                            Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Ошибка аутентификации");
                    }
                }
            }
            catch
            { MessageBox.Show("Произошла ошибка. Проверьте подключение к интернету!"); }
        }

        private void buttonFAQ_Click(object sender, RoutedEventArgs e)
        {
            string resourcePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "Resources", "faq.pdf");
            Process.Start(resourcePath);
        }
    }
}
