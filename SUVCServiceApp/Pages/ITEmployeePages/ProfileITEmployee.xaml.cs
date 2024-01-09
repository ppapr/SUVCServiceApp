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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SUVCServiceApp.Pages.ITEmployeePages
{
    /// <summary>
    /// Логика взаимодействия для ProfileITEmployee.xaml
    /// </summary>
    public partial class ProfileITEmployee : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        private ResponseUsers authenticatedUser;
        public ProfileITEmployee(ResponseUsers authenticatedUser)
        {
            InitializeComponent();
            this.authenticatedUser = authenticatedUser;
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadDataGrid();
            LoadUserInfo();
        }
        void LoadUserInfo()
        {
            textBoxName.Text = authenticatedUser.Name;
            textBoxSurName.Text = authenticatedUser.Surname;
            textBoxLogin.Text = authenticatedUser.Login;
            textBoxPassword.Text = authenticatedUser.Password;
            textBoxMiddleName.Text = authenticatedUser.MiddleName;
        }
        private async void LoadDataGrid()
        {
            var requestsUser = await apiDataProvider.GetDataFromApi<ResponseRequests>($"Requests?userExecutor={authenticatedUser.ID}");
            if (requestsUser != null)
            {
                List<ResponseRequests> requestsHistory = requestsUser.Where(x => x.IDStatus == 3).ToList();
                listViewHistory.ItemsSource = requestsHistory;
            }
        }

        private async void buttonSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ApiDataProvider apiDataProvider = new ApiDataProvider();
                int currentUserID = authenticatedUser.ID;
                ResponseUsers user = new ResponseUsers
                {
                    ID = authenticatedUser.ID,
                    Name = textBoxName.Text,
                    Surname = textBoxSurName.Text,
                    MiddleName = textBoxMiddleName.Text,
                    Login = textBoxLogin.Text,
                    Password = textBoxPassword.Text,
                    IDRole = authenticatedUser.IDRole,
                };

                bool isSuccess = await apiDataProvider.UpdateDataToApi("Users", authenticatedUser.ID, user);
                if (isSuccess)
                {
                    MessageBox.Show($"Изменения сотрудника {authenticatedUser.FullName} произошли успешно! \nИзменения вступят в силу после перезапуска программы!");
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
