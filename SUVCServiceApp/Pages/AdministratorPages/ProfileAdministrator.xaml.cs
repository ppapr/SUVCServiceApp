using SUVCServiceApp.Controller;
using SUVCServiceApp.ViewModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using MessageBox = System.Windows.Forms.MessageBox;

namespace SUVCServiceApp.Pages.AdministratorPages
{
    /// <summary>
    /// Логика взаимодействия для ProfileAdministrator.xaml
    /// </summary>
    public partial class ProfileAdministrator : Page
    {
        ResponseUsers currentUser;
        public ProfileAdministrator(ResponseUsers response)
        {
            InitializeComponent();
            this.currentUser = response;
            LoadReportsToListView();
            LoadUserInfo();
        }

        void LoadUserInfo()
        {
            textBoxName.Text = currentUser.Name;
            textBoxSurName.Text = currentUser.Surname;
            textBoxLogin.Text = currentUser.Login;
            textBoxPassword.Text = currentUser.Password;
            textBoxMiddleName.Text = currentUser.MiddleName;
        }

        private void LoadReportsToListView()
        {
            string reportsFolderPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports");
            if (Directory.Exists(reportsFolderPath))
            {
                string[] reportFiles = Directory.GetFiles(reportsFolderPath, "*.pdf");
                listViewReports.Items.Clear();
                foreach (string reportFile in reportFiles)
                {
                    listViewReports.Items.Add(System.IO.Path.GetFileName(reportFile));
                }
            }
        }
        private string selectedReport;

        private void listViewReports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedReport = listViewReports.SelectedItem as string;
        }

        private void buttonCreateReport_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(selectedReport))
            {
                string reportFilePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Reports", selectedReport);
                Process.Start(reportFilePath);
            }
            else
            {
                MessageBox.Show("Выберите отчет из списка.");
            }
        }

        private async void buttonSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ApiDataProvider apiDataProvider = new ApiDataProvider();
                int currentUserID = currentUser.ID;
                ResponseUsers user = new ResponseUsers
                {
                    ID = currentUser.ID,
                    Name = textBoxName.Text,
                    Surname = textBoxSurName.Text,
                    MiddleName = textBoxMiddleName.Text,
                    Login = textBoxLogin.Text,
                    Password = textBoxPassword.Text,
                    IDRole = currentUser.IDRole,
                };

                bool isSuccess = await apiDataProvider.UpdateDataToApi("Users", currentUserID, user);
                if (isSuccess)
                {
                    MessageBox.Show($"Изменения сотрудника {currentUser.FullName} произошли успешно! \nИзменения вступят в силу после перезапуска программы!");
                    LoadUserInfo();
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
