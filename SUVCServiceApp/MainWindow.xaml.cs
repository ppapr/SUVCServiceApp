using Newtonsoft.Json;
using SUVCServiceApp.ViewModel;
using SUVCServiceApp.Windows;
using System;
using System.Collections.Generic;
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

            using (HttpClient client = new HttpClient())
            {
                // Замените URL на соответствующий
                string apiUrl = $"http://localhost:61895/api/Users?login={login}&password={password}";

                HttpResponseMessage response = await client.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                    ResponseUsers authenticatedUser = JsonConvert.DeserializeObject<ResponseUsers>(responseData);
                    new AdministratorWindow().Show();
                    Close();
                    // Здесь вы можете использовать информацию о пользователе, если аутентификация прошла успешно
                }
                else
                {
                    // Обработка ошибок аутентификации
                    MessageBox.Show("Ошибка аутентификации");
                }
            }
        }
    }
}
