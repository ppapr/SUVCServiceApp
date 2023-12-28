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
    /// Логика взаимодействия для RequestITEmployeePage.xaml
    /// </summary>
    public partial class RequestITEmployeePage : Page
    {
        private readonly ApiDataProvider apiDataProvider = new ApiDataProvider();
        private readonly DataGridLoader dataGridLoader;
        private ResponseUsers authenticatedUser;

        public RequestITEmployeePage(ResponseUsers authenticatedUser)
        {
            InitializeComponent();
            this.authenticatedUser = authenticatedUser;
            dataGridLoader = new DataGridLoader(apiDataProvider);
            LoadDataGrid();
        }

        private async void LoadDataGrid()
        {
            await dataGridLoader.LoadData<ResponseRequests>(listRequests, $"Requests?userExecutor={authenticatedUser}");
            await dataGridLoader.LoadData<ResponseRequests>(listRequests, $"Requests?userExecutor=10");
        }

        private void buttonShowMap_Click(object sender, RoutedEventArgs e)
        {
            new Windows.InteractiveMapWindow().ShowDialog();
        }
        private ResponseRequests currentRequest;
        private void listRequests_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentRequest = (ResponseRequests)listRequests.SelectedItem;
        }

        async Task UpdateStatusRequest(int statusID)
        {
            try
            {
                ApiDataProvider apiDataProvider = new ApiDataProvider();
                int currentRequestID = currentRequest.ID;
                ResponseRequests request = new ResponseRequests
                {
                    ID = currentRequest.ID,
                    Description = currentRequest.Description,
                    DateCreateRequest = currentRequest.DateCreateRequest,
                    DateExecuteRequest = currentRequest.DateExecuteRequest,
                    IDStatus = statusID,
                    IDPriority = currentRequest.IDPriority,
                    IDEquipment = currentRequest.IDEquipment,
                    IDUserRequest = currentRequest.IDUserRequest,
                    IDExecutorRequest = currentRequest.IDExecutorRequest
                };

                bool isSuccess = await apiDataProvider.UpdateDataToApi("Requests", currentRequestID, request);
                if (isSuccess)
                {
                    MessageBox.Show($"Вы изменили статус заявки сотрудника {currentRequest.UserRequestName}");
                }
                else
                {
                    MessageBox.Show("Произошла ошибка! Проверьте поля ввода данных!");
                }
            }
            catch
            {
                MessageBox.Show("Проверьте корректность данных и соеднинение с интернетом!");
            }
        }

        private async void buttonStartExecute_Click(object sender, RoutedEventArgs e)
        {
            await UpdateStatusRequest(2);
        }

        private async void buttonEndExecute_Click(object sender, RoutedEventArgs e)
        {
            await UpdateStatusRequest(3);
        }

        private async void textBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchTerm = textBoxSearch.Text;
            Func<ResponseRequests, string> searchProperty = item => item.Description;
            await dataGridLoader.LoadData(listRequests, "Requests", searchProperty, searchTerm);
        }
    }
}
