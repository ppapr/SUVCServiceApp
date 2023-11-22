using SUVCServiceApp.Controller;
using SUVCServiceApp.ViewModel;
using SUVCServiceApp.Windows;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SUVCServiceApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEmployeePage.xaml
    /// </summary>
    public partial class AddEmployeePage : Page
    {
        private readonly AdministratorWindow administratorWindow;
        public AddEmployeePage(AdministratorWindow administratorWindow)
        {
            InitializeComponent(); 
            this.administratorWindow = administratorWindow;
        }

        private async void buttonAddUser_Click(object sender, RoutedEventArgs e)
        {
            int role = comboBoxRole.Text == "Сотрудник" ? 3 : 2;
            ApiDataProvider apiDataProvider = new ApiDataProvider();
            ResponseUsers user = new ResponseUsers
            {
                Name = textBoxName.Text,
                Surname = textBoxSurName.Text,
                MiddleName = textBoxMiddleName.Text,
                Login = textBoxLogin.Text,
                Password = textBoxPassword.Text,
                IDRole = role,
            };

            bool isSuccess = await apiDataProvider.AddDataToApi("Users", user);
            if (isSuccess)
            {
                MessageBox.Show($"Сотрудник {textBoxSurName.Text} {textBoxName.Text} {textBoxMiddleName.Text} успешно добавлен!");
            }
            else
            {
                MessageBox.Show("Произошла ошибка при добавлении данных! Проверьте поля ввода данных!");
            }
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            administratorWindow.FrameWorkspace.Navigate(new EmployeePage(administratorWindow));
        }
    }
}
