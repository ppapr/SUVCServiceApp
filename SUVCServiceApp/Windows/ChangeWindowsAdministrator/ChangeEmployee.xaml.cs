using SUVCServiceApp.Controller;
using SUVCServiceApp.ViewModel;
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
using System.Windows.Shapes;

namespace SUVCServiceApp.Windows.ChangeWIndowsAdministrator
{
    /// <summary>
    /// Логика взаимодействия для ChangeEmployee.xaml
    /// </summary>
    public partial class ChangeEmployee : Window
    {
        ResponseUsers response;
        public ChangeEmployee(ResponseUsers responseUsers)
        {
            InitializeComponent();
            this.response = responseUsers;
            textBoxName.Text = response.Name;
            textBoxMiddleName.Text = response.MiddleName;
            textBoxLogin.Text = response.Login;
            textBoxSurName.Text = response.Surname;
            textBoxPassword.Text = response.Password;
        }

        private void buttonExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void buttonSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ApiDataProvider apiDataProvider = new ApiDataProvider();
                int currentUserID = response.ID;
                ResponseUsers user = new ResponseUsers
                {
                    ID = response.ID,
                    Name = textBoxName.Text,
                    Surname = textBoxSurName.Text,
                    MiddleName = textBoxMiddleName.Text,
                    Login = textBoxLogin.Text,
                    Password = textBoxPassword.Text,
                    IDRole = response.IDRole,
                };

                bool isSuccess = await apiDataProvider.UpdateDataToApi("Users", currentUserID, user);
                if (isSuccess)
                {
                    MessageBox.Show($"Изменения сотрудника {response.FullName} произошли успешно!");
                }
                else
                {
                    MessageBox.Show("Произошла ошибка при добавлении данных! Проверьте поля ввода данных!");
                }
            }
            catch
            {
                MessageBox.Show("Проверьте заполненность данных и соеднинение с интернетом!");
            }
        }
    }
}
